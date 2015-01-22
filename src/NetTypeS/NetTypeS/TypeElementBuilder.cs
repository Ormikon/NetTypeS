using System;
using System.Collections.Generic;
using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;
using NetTypeS.Types;
using NetTypeS.Utils;

namespace NetTypeS
{
	internal class TypeElementBuilder : ITypeElementBuilder
	{
		private readonly string moduleName;
		private readonly ITypeCollector collector;
		private readonly ICustomTypeNameHolder customNameHolder;
		private readonly IGeneratorSettings settings;

		public TypeElementBuilder(string moduleName, ITypeCollector collector,
			ICustomTypeNameHolder customTypeNameHolder, IGeneratorSettings settings)
		{
			this.moduleName = moduleName ?? "";
			this.collector = collector;
			customNameHolder = customTypeNameHolder;
			this.settings = settings;
		}

		private ITypeScriptElement GetCustomOrAnyNameElement(Type type = null, ITypeScriptType tsType = null)
		{
			if (type == null)
				return new TypeNameElement(settings.TypeNameResolver(SimpleType.Any));
			string name = customNameHolder.GetNameFor(type);
			return new TypeNameElement(name ?? settings.TypeNameResolver(tsType ?? SimpleType.Any));
		}

		private ITypeScriptElement GetComplexTypeElement(IComplexType ct)
		{
			string name;
			if (!ct.IsGeneric)
			{
				name = customNameHolder.GetNameFor(ct.Type) ?? settings.TypeNameResolver(ct);
				return new InterfaceNameElement(name);
			}
			name = customNameHolder.GetNameFor(ct.IsGenericDefinition ? ct.Type : ct.GenericType);
			if (string.IsNullOrEmpty(name))
			{
				if (ct.IsGenericDefinition)
					name = settings.TypeNameResolver(ct);
				else
				{
					var gct = collector.Get(ct.GenericType);
					name = settings.TypeNameResolver(gct);
				}
			}

			var elements = new List<ITypeScriptElement> {new InterfaceNameElement(name), new TextElement("<")};
			for (int i = 0; i < ct.GenericArguments.Length; i++)
			{
				if (i > 0)
					elements.Add(new TextElement(","));
				var ga = ct.GenericArguments[i];
				elements.Add(new TypeLinkElement(ga));
			}
			elements.Add(new TextElement(">"));

			return new FixedElement(elements);
		}

		public ITypeScriptElement GetTypeNameElement(Type type)
		{
			if (type == null)
				return GetCustomOrAnyNameElement();
			var tst = collector.Get(type);
			if (tst == null)
			{
				// Auto generate name for constructed generic if collected
				if (type.IsGenericType && type.IsConstructedGenericType)
				{
					tst = collector.Get(type.GetGenericTypeDefinition());
					if (tst != null && tst.Code == TypeScriptTypeCode.Complex)
					{
						return GetComplexTypeElement(new ComplexType(type));
					}
				}
				else if (type.IsNullable())
					return GetTypeNameElement(Nullable.GetUnderlyingType(type));
				return GetCustomOrAnyNameElement(type);
			}
			if (tst.Code == TypeScriptTypeCode.Simple)
				return GetCustomOrAnyNameElement(type, tst);
			if (tst.Code == TypeScriptTypeCode.Nullable)
				return GetTypeNameElement(((NullableType) tst).UnderlyingType);
			if (tst.Code == TypeScriptTypeCode.Collection)
				return new FixedElement(GetTypeNameElement(((CollectionType) tst).Type), new TextElement("[]"));
            if (tst.Code == TypeScriptTypeCode.Dictionary)
                return new FixedElement(
                    new TextElement("{ [ key: "),
                    GetTypeNameElement(((DictionaryType)tst).KeyType),
                    new TextElement(" ] : "),
                    GetTypeNameElement(((DictionaryType)tst).ValueType),
                    new TextElement(" }"));
            if (tst.Code == TypeScriptTypeCode.Enum)
			{
				string customName = customNameHolder.GetNameFor(type);
				return new EnumNameElement(customName ?? settings.TypeNameResolver(tst));
			}
			if (tst.Code == TypeScriptTypeCode.Complex)
			{
				return GetComplexTypeElement((ComplexType) tst);
			}
			throw new NotSupportedException("Not supported TypeScript type code provided " + tst.Code + ".");
		}

		private string GetModuleLink(string moduleBinding)
		{
			if (moduleBinding == null)
				moduleBinding = "";
			// If the same module or parent
			if (moduleBinding.Equals(moduleName, StringComparison.Ordinal) ||
			    moduleName.StartsWith(moduleBinding + ".", StringComparison.Ordinal))
				return "";
			// If child
			if (!string.IsNullOrEmpty(moduleName) && moduleBinding.StartsWith(moduleName + ".", StringComparison.Ordinal))
			{
				var prefix = moduleBinding.Substring(moduleName.Length + 1);
				return prefix;
			}
			// Full path
			return moduleBinding;
		}

		public ITypeScriptElement GetTypeModuleElement(Type type)
		{
			if (type == null)
				return null;
			var tst = collector.Get(type);
			if (tst == null)
			{
				// Auto generate name for constructed generic if collected
				if (type.IsGenericType && type.IsConstructedGenericType)
				{
					return GetTypeModuleElement(type.GetGenericTypeDefinition());
				}
				if (type.IsNullable())
					return GetTypeModuleElement(Nullable.GetUnderlyingType(type));
				return null;
			}
			if (tst.Code == TypeScriptTypeCode.Nullable)
				return GetTypeModuleElement(((NullableType) tst).UnderlyingType);
			if (tst.Code == TypeScriptTypeCode.Collection)
			{
				return GetTypeModuleElement(((ICollectionType) tst).Type);
			}
            if (tst.Code == TypeScriptTypeCode.Dictionary)
			{
                // TODO: use key, value or do something more complex?
				return GetTypeModuleElement(((IDictionaryType) tst).KeyType);
			}
			if (tst.Code == TypeScriptTypeCode.Complex)
			{
				var ct = (IComplexType) tst;
				if (ct.IsGeneric && !ct.IsGenericDefinition)
				{
					type = ct.GenericType;
					tst = collector.Get(type);
				}
			}
			if (tst.Code == TypeScriptTypeCode.Enum || tst.Code == TypeScriptTypeCode.Complex)
			{
				var binding = collector.GetModuleBinding(type);
				var link = GetModuleLink(binding);
				return string.IsNullOrEmpty(link) ? null : new ModuleNameElement(link);
			}
			return null;
		}
	}
}