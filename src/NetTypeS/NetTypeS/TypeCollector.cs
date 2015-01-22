using System;
using System.Collections.Generic;
using System.Linq;
using NetTypeS.Interfaces;
using NetTypeS.Types;
using NetTypeS.Utils;

namespace NetTypeS
{
	public class TypeCollector : ITypeCollector
	{
		private class CollectedTypeInfo
		{
			private readonly ITypeScriptType type;

			public CollectedTypeInfo(ITypeScriptType type, string moduleBinding = null)
			{
				this.type = type;
				ModuleBinding = moduleBinding;
			}

			public ITypeScriptType Type
			{
				get { return type; }
			}

			public string ModuleBinding { get; set; }
		}

		private readonly IInheritedTypeSpy inheritedTypeSpy;
		private readonly bool includeInheritedTypes;
        private readonly bool generateNumberTypeForDictionaryKeys;
		private readonly ISet<TypeMutator> mutators = new HashSet<TypeMutator>();
		private readonly IDictionary<Type, CollectedTypeInfo> collected = new Dictionary<Type, CollectedTypeInfo>();

		public TypeCollector(IInheritedTypeSpy inheritedTypeSpy = null, 
            bool includeInherited = false,
            bool generateNumberTypeForDictionaryKeys = false)
		{
			this.inheritedTypeSpy = inheritedTypeSpy;
			includeInheritedTypes = includeInherited;
            this.generateNumberTypeForDictionaryKeys = generateNumberTypeForDictionaryKeys;
			RegisterSimpleTypes();
			// Add replacement for basic types
			foreach (var kv in TypeUtils.KnownTypes)
			{
				Replace(kv.Key, kv.Value);
			}
		}

		private void RegisterSimpleTypes()
		{
			Collect(typeof (object), string.Empty);
			Collect(typeof (void), string.Empty);
			Collect(typeof (bool), string.Empty);
			Collect(typeof (Byte), string.Empty);
			Collect(typeof (SByte), string.Empty);
			Collect(typeof (decimal), string.Empty);
			Collect(typeof (double), string.Empty);
			Collect(typeof (float), string.Empty);
			Collect(typeof (Int16), string.Empty);
			Collect(typeof (Int32), string.Empty);
			Collect(typeof (Int64), string.Empty);
			Collect(typeof (UInt16), string.Empty);
			Collect(typeof (UInt32), string.Empty);
			Collect(typeof (UInt64), string.Empty);
			Collect(typeof (char), string.Empty);
			Collect(typeof (string), string.Empty);
		}

		public void Collect(Type type, string moduleBinding, bool overrideBindingIfExists = true)
		{
			Collect(type, includeInheritedTypes, moduleBinding, overrideBindingIfExists);
		}

		public void Collect(Type type, bool includeInherited, string moduleBinding, bool overrideBindingIfExists = true)
		{
			// TODO: Recursion to cycle?
			if (type == null)
				return;

			type.ThrowIfUnsupported();
			if (collected.ContainsKey(type))
			{
				if (overrideBindingIfExists)
				{
					var cti = collected[type];
					if (cti.Type.Code == TypeScriptTypeCode.Nullable)
						cti = collected[((NullableType) cti.Type).UnderlyingType];
					if (cti.Type.Code == TypeScriptTypeCode.Complex || cti.Type.Code == TypeScriptTypeCode.Enum)
					{
						cti.ModuleBinding = moduleBinding;
					}
				}
				return;
			}

			// Generic arguments in most cases cannot be mutated
			if (type.IsGenericParameter)
			{
				collected.Add(type, new CollectedTypeInfo(SimpleType.Create(type)));
				return;
			}

			if (Mutate(type, moduleBinding))
				return;

			if (type.IsNullable())
			{
				var nut = Nullable.GetUnderlyingType(type);
				collected.Add(type, new CollectedTypeInfo(new NullableType(nut)));
				// The same method parameters if nullable
				Collect(nut, includeInherited, moduleBinding, overrideBindingIfExists);
				return;
			}

			if (type.IsEnum())
				collected.Add(type, new CollectedTypeInfo(new EnumType(type), moduleBinding));
			else if (type.IsSimple())
				collected.Add(type, new CollectedTypeInfo(SimpleType.Create(type)));
            else if (type.IsCollection())
			{
                if (type.IsDictionary())
                {
                    var dictTypeParams = type.GetDictionaryTypes();
                    var keyType = dictTypeParams[0];
                    Type tsKeyType = (keyType.IsNumber() && generateNumberTypeForDictionaryKeys) 
                        ? typeof(int) 
                        : typeof(string);
                    var dt = new DictionaryType(tsKeyType, dictTypeParams[1]);
                    collected.Add(type, new CollectedTypeInfo(dt));
                    Collect(dt.ValueType, includeInherited, moduleBinding, false);
                }
                else
                {
                    var ct = new CollectionType(type);
                    collected.Add(type, new CollectedTypeInfo(ct));
                    Collect(ct.Type, includeInherited, moduleBinding, false);
                }
			}
			else
			{
				CollectComplex(type, includeInherited, moduleBinding, overrideBindingIfExists);
			}
		}

		private void CollectNonGenericComplex(IComplexType type, bool includeInherited, string moduleBinding,
			bool overrideBindingIfExists)
		{
			collected.Add(type.Type, new CollectedTypeInfo(type, moduleBinding));
			if (type.IsInterface)
				foreach (var @interface in type.Interfaces)
				{
					// Do not include inherited for implemented interfaces
					Collect(@interface, false, moduleBinding, overrideBindingIfExists);
				}
			foreach (var p in type.Properties)
			{
				Collect(p.Type, includeInherited, moduleBinding, false);
			}
		}

		private void CollectComplex(Type type, bool includeInherited, string moduleBinding, bool overrideBindingIfExists)
		{
			if (type.IsGenericType)
			{
				// We do not collecting information for constructed types because of
				// possible circle references
                if (type.IsConstructedGenericType)
                {
                    foreach(var genericArg in type.GenericTypeArguments)
                    {
                        Collect(genericArg, includeInherited, moduleBinding, overrideBindingIfExists);
                    }

                    Collect(type.GetGenericTypeDefinition(), includeInherited, moduleBinding, overrideBindingIfExists);
                }
                else
				{
					var ct = new ComplexType(type);
					CollectNonGenericComplex(ct, includeInherited, moduleBinding, overrideBindingIfExists);
					foreach (var genericArgument in ct.GenericArguments)
					{
						Collect(genericArgument, includeInherited, moduleBinding, overrideBindingIfExists);
					}
				}
			}
			else
			{
				var ct = new ComplexType(type);
				CollectNonGenericComplex(ct, includeInherited, moduleBinding, overrideBindingIfExists);
			}
			if (includeInherited && inheritedTypeSpy != null)
			{
				foreach (var inheritedType in inheritedTypeSpy.GetInheritedTypes(type))
				{
					Collect(inheritedType, true, moduleBinding, overrideBindingIfExists);
				}
			}
		}

		private bool Mutate(Type type, string moduleBinding)
		{
			// Mutators logic
			Type mutated;
			if (TryMutate(type, out mutated))
			{
				// Save reference before next collect to prevent circular mutations
				collected.Add(type, null);
				// Do not override binding for mutated types
				Collect(mutated, moduleBinding, false);
				var cti = GetInfo(mutated);
				if (cti == null)
				{
					collected.Remove(type);
					throw new InvalidOperationException(
						"Unable to collect information about " + type.FullName +
						" type. Some of type replacements replace type with the same type and " +
						" it makes circular replacement references. Please check the code of registered" +
						" Replace delegates and remove circular type replacement.");
				}
				collected[type] = cti;
				return true;
			}

			return false;
		}

		private bool TryMutate(Type type, out Type replacement)
		{
			replacement = null;
			foreach (var mutator in mutators)
			{
				if (mutator.TryMutate(type, out replacement))
					return true;
			}
			return false;
		}

		public void Replace(Type type, Type withType)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			if (withType == null)
				throw new ArgumentNullException("withType");

			if (type == withType)
				return;

			var cti = GetInfo(withType);
			if (cti == null)
				Collect(withType, "", false);
			cti = GetInfo(withType);
			if (cti == null)
				throw new InvalidOperationException("For some reason it is unable to collect information for type " +
				                                    withType.FullName);
			collected[type] = cti;
		}

		public void Replace(Func<Type, bool> test, Type withType)
		{
			if (test == null)
				throw new ArgumentNullException("test");
			if (withType == null)
				throw new ArgumentNullException("withType");

			var mutator = new TypeMutator(test, withType);
			if (mutators.Contains(mutator))
				return;

			mutators.Add(mutator);

			var c = collected.Keys.ToArray();
			foreach (var type in c)
			{
				Type newType;
				if (mutator.TryMutate(type, out newType))
				{
					var cti = GetInfo(newType);
					if (cti == null)
						Collect(newType, "", false);
					cti = GetInfo(newType);
					if (cti == null)
						throw new InvalidOperationException("For some reason it is unable to collect information for type " +
						                                    withType.FullName);
					collected[type] = cti;
				}
			}
		}

		private CollectedTypeInfo GetInfo(Type type)
		{
			if (type == null)
				return null;
			CollectedTypeInfo cti;
			return collected.TryGetValue(type, out cti) ? cti : null;
		}

		public ITypeScriptType Get(Type type)
		{
			if (type == null)
				return null;
			var cti = GetInfo(type);
			return cti == null ? null : cti.Type;
		}

		public string GetModuleBinding(Type type)
		{
			if (type == null)
				return null;
			var cti = GetInfo(type);
			return cti == null ? null : cti.ModuleBinding;
		}

		public IEnumerable<IComplexType> GetComplexTypes()
		{
			return Collected
				.Where(tst => tst.Code == TypeScriptTypeCode.Complex)
				.Cast<ComplexType>()
				.Where(ct => !ct.IsGeneric || ct.IsGenericDefinition);
		}

		public IEnumerable<IComplexType> GetComplexTypes(string module)
		{
			return GetTypes(module)
				.Where(tst => tst.Code == TypeScriptTypeCode.Complex)
				.Cast<ComplexType>()
				.Where(ct => !ct.IsGeneric || ct.IsGenericDefinition);
		}

		public IEnumerable<IEnumType> GetEnumTypes()
		{
			return Collected.Where(tst => tst.Code == TypeScriptTypeCode.Enum).Cast<IEnumType>();
		}

		public IEnumerable<IEnumType> GetEnumTypes(string module)
		{
			return GetTypes(module).Where(tst => tst.Code == TypeScriptTypeCode.Enum).Cast<IEnumType>();
		}

		private static bool CompareModuleBindings(string binding1, string binding2)
		{
			return string.IsNullOrEmpty(binding1)
				? string.IsNullOrEmpty(binding2)
				: !string.IsNullOrEmpty(binding2) && binding1.Equals(binding2, StringComparison.Ordinal);
		}

		public IEnumerable<ITypeScriptType> GetTypes(string module)
		{
			return collected.Values
				.Distinct()
				.Where(cti => CompareModuleBindings(cti.ModuleBinding, module))
				.Select(cti => cti.Type);
		}

		public IEnumerable<ITypeScriptType> Collected
		{
			get { return collected.Values.Select(cti => cti.Type); }
		}

		public IEnumerable<Type> CollectedTypes
		{
			get { return collected.Keys; }
		}
	}
}