using System;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS.Types
{
	internal sealed class DictionaryType : IDictionaryType
	{
		private readonly Type keyType;
        private readonly Type valueType;
        private readonly string @namespace;

        public DictionaryType(Type keyType, Type valueType)
		{
            this.keyType = keyType;
            this.valueType = valueType;
			@namespace = valueType.Namespace;
		}

		public override string ToString()
		{
			return "Dictionary(" + keyType.Name + ", " + valueType.Name + ")";
		}

		public Type KeyType
		{
			get { return keyType; }
		}

        public Type ValueType
        {
            get { return valueType; }
        }

		public string Name
		{
			get { return ""; }
		}

		public string Namespace
		{
			get { return @namespace; }
		}

		public string FullName
		{
			get { return @namespace + ".[]"; }
		}

		public TypeScriptTypeCode Code
		{
			get { return TypeScriptTypeCode.Dictionary; }
		}

		public bool IsRequired
		{
			get { return false; }
		}

		public Attribute[] CustomAttributes
		{
			get { return new Attribute[0]; }
		}
	}
}