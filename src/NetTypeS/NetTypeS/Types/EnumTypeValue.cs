using System;
using System.Linq;
using System.Reflection;
using NetTypeS.Interfaces;

namespace NetTypeS.Types
{
	internal class EnumTypeValue : IEnumValue
	{
		private readonly string name;
		private readonly object value;
		private readonly Lazy<Attribute[]> customAttributes;

		public EnumTypeValue(string name, object value, MemberInfo enumMemberInfo)
		{
			this.name = name;
			this.value = value;
			customAttributes = new Lazy<Attribute[]>(() => enumMemberInfo.GetCustomAttributes<Attribute>().ToArray());
		}

		public long ValueAsInt64()
		{
			if (value == null)
				throw new InvalidOperationException("Enumerable element value is not assigned.");
			return Convert.ToInt64(value);
		}

		public string Name
		{
			get { return name; }
		}

		public object Value
		{
			get { return value; }
		}

		public bool HasValue
		{
			get { return value != null; }
		}

		public Attribute[] CustomAttributes
		{
			get { return customAttributes.Value; }
		}
	}
}