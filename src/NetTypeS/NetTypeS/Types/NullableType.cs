using System;
using NetTypeS.Interfaces;

namespace NetTypeS.Types
{
    internal class NullableType : INullableType
    {
        public NullableType(Type underlyingType)
        {
            UnderlyingType = underlyingType;
        }

        public override string ToString()
        {
            return "Nullable(" + UnderlyingType.Name + ")";
        }

        public Type UnderlyingType { get; }

        public string Name => "";

        public string Namespace => "";

        public string FullName => "";

        public TypeScriptTypeCode Code => TypeScriptTypeCode.Nullable;

        public bool IsRequired => false;

        public Attribute[] CustomAttributes => new Attribute[0];
    }
}