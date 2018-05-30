using System;
using NetTypeS.Interfaces;

namespace NetTypeS.Types
{
    internal sealed class DictionaryType : IDictionaryType
    {
        public DictionaryType(Type keyType, Type valueType)
        {
            KeyType = keyType;
            ValueType = valueType;
            Namespace = valueType.Namespace;
        }

        public override string ToString()
        {
            return "Dictionary(" + KeyType.Name + ", " + ValueType.Name + ")";
        }

        public Type KeyType { get; }

        public Type ValueType { get; }

        public string Name => "";

        public string Namespace { get; }

        public string FullName => Namespace + ".[]";

        public TypeScriptTypeCode Code => TypeScriptTypeCode.Dictionary;

        public bool IsRequired => false;

        public Attribute[] CustomAttributes => new Attribute[0];
    }
}