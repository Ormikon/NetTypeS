using System;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS.Types
{
    internal sealed class CollectionType : ICollectionType
    {
        public CollectionType(Type type)
        {
            Type = type.GetCollectionType();
            if (Type == null)
                throw new ArgumentException("Invalid collection type specified.", "type");
            Namespace = type.Namespace;
        }

        public override string ToString()
        {
            return "Collection(" + Type.Name + "[])";
        }

        public Type Type { get; }

        public string Name => "";

        public string Namespace { get; }

        public string FullName => Namespace + ".[]";

        public TypeScriptTypeCode Code => TypeScriptTypeCode.Collection;

        public bool IsRequired => false;

        public Attribute[] CustomAttributes => new Attribute[0];
    }
}