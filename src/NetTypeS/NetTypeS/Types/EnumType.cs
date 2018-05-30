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
        private readonly Lazy<Attribute[]> customAttributes;

        public EnumType(Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Only Enum types supported.", "type");

            Type = type;
            Name = type.Name;
            Namespace = type.Namespace;
            Values = type.GetEnumTypeValues();
            customAttributes = new Lazy<Attribute[]>(() => type.GetCustomAttributes<Attribute>().ToArray());
        }

        public override string ToString()
        {
            return "Enum(" + Name + "[" + string.Join(", ", Values) + "])";
        }

        public Type Type { get; }

        public string Name { get; }

        public string Namespace { get; }

        public string FullName => Namespace + "." + Name;

        public IReadOnlyList<IEnumValue> Values { get; }

        public TypeScriptTypeCode Code => TypeScriptTypeCode.Enum;

        public bool IsRequired => true;

        public Attribute[] CustomAttributes => customAttributes.Value;
    }
}