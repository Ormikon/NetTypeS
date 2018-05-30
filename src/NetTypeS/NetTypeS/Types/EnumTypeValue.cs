using System;
using System.Linq;
using System.Reflection;
using NetTypeS.Interfaces;

namespace NetTypeS.Types
{
    internal class EnumTypeValue : IEnumValue
    {
        private readonly Lazy<Attribute[]> customAttributes;

        public EnumTypeValue(string name, object value, MemberInfo enumMemberInfo)
        {
            Name = name;
            Value = value;
            customAttributes = new Lazy<Attribute[]>(() => enumMemberInfo.GetCustomAttributes<Attribute>().ToArray());
        }

        public long ValueAsInt64()
        {
            if (Value == null)
                throw new InvalidOperationException("Enumerable element value is not assigned.");
            return Convert.ToInt64(Value);
        }

        public string Name { get; }

        public object Value { get; }

        public bool HasValue => Value != null;

        public Attribute[] CustomAttributes => customAttributes.Value;
    }
}