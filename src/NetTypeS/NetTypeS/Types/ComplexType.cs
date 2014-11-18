using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS.Types
{
	internal sealed class ComplexType : IComplexType
	{
		private readonly Type type;
		private readonly string name;
		private readonly string @namespace;
		private readonly bool isRequired;
		private readonly bool isValueType;
		private readonly bool isGenerated;
		private readonly IReadOnlyList<ITypeProperty> properties;
		private readonly Lazy<Attribute[]> customAttributes;
		private readonly Type[] interfaces;
		private readonly bool isInterface;

		public ComplexType(Type type)
		{
			isRequired = false;
			if (type.IsValueType)
			{
				isValueType = true;
				isRequired = true;
				if (type.IsNullable())
				{
					throw new NotSupportedException("Nullable types are not supported in this class.");
				}
			}
			else
				isValueType = false;

			this.type = type;
			name = type.IsGenericType ? GetGenericTypeName(type.Name) : type.Name;
			@namespace = type.Namespace;

			isGenerated = type.IsDefined(typeof (CompilerGeneratedAttribute), false);

			interfaces = type.GetInterfaces();
			isInterface = type.IsInterface;
			properties = type.GetPublicMembers().Select(mi => new ComplexTypeProperty(mi)).ToArray();
			customAttributes = new Lazy<Attribute[]>(() => type.GetCustomAttributes<Attribute>().ToArray());
		}

		private static string GetGenericTypeName(string name)
		{
			int generatedIdx = name.IndexOf('`');
			return generatedIdx < 0 ? name : name.Remove(generatedIdx);
		}

		public override string ToString()
		{
			return "Complex[" + (isValueType ? "struct" : "class") + "](" + name + "[" +
			       string.Join("; ",
				       properties.Select(
					       p =>
						       p.Name + "{" + (p.CanRead ? "get" + (p.CanWrite ? ";set" : "") : p.CanWrite ? "set" : "") +
						       "}")) +
			       "])";
		}

		public Type Type
		{
			get { return type; }
		}

		public string Name
		{
			get { return name; }
		}

		public string Namespace
		{
			get { return @namespace; }
		}

		public string FullName
		{
			get { return @namespace + "." + name; }
		}

		public bool IsValueType
		{
			get { return isValueType; }
		}

		public bool IsGenerated
		{
			get { return isGenerated; }
		}

		public bool IsGeneric
		{
			get { return type.IsGenericType; }
		}

		public bool IsGenericDefinition
		{
			get { return type.IsGenericTypeDefinition; }
		}

		public bool IsInterface
		{
			get { return isInterface; }
		}

		public Type GenericType
		{
			get { return type.GetGenericTypeDefinition(); }
		}

		public Type[] GenericArguments
		{
			get { return type.GetGenericArguments(); }
		}

		public Type[] Interfaces
		{
			get { return interfaces; }
		}

		public IReadOnlyList<ITypeProperty> Properties
		{
			get { return properties; }
		}

		public TypeScriptTypeCode Code
		{
			get { return TypeScriptTypeCode.Complex; }
		}

		public bool IsRequired
		{
			get { return isRequired; }
		}

		public Attribute[] CustomAttributes
		{
			get { return customAttributes.Value; }
		}
	}
}