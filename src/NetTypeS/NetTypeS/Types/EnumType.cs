using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS.Types
{
	internal sealed class EnumType : IEnumType
	{
		private readonly Type type;
		private readonly string name;
		private readonly string @namespace;
		private readonly IReadOnlyList<IEnumValue> values;
		private readonly Lazy<Attribute[]> customAttributes;

		public EnumType(Type type)
		{
			if (!type.IsEnum)
				throw new ArgumentException("Only Enum types supported.", "type");

			this.type = type;
			name = type.Name;
			@namespace = type.Namespace;
			values = type.GetEnumTypeValues();
			customAttributes = new Lazy<Attribute[]>(() => type.GetCustomAttributes<Attribute>().ToArray());
		}

		public override string ToString()
		{
			return "Enum(" + name + "[" + string.Join(", ", Values) + "])";
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

		public IReadOnlyList<IEnumValue> Values
		{
			get { return values; }
		}

		public TypeScriptTypeCode Code
		{
			get { return TypeScriptTypeCode.Enum; }
		}

		public bool IsRequired
		{
			get { return true; }
		}

		public Attribute[] CustomAttributes
		{
			get { return customAttributes.Value; }
		}
	}
}