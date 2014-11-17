using System;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS.Types
{
	internal sealed class CollectionType : ICollectionType
	{
		private readonly Type collectionType;
		private readonly string @namespace;

		public CollectionType(Type type)
		{
			collectionType = type.GetCollectionType();
			if (collectionType == null)
				throw new ArgumentException("Invalid collection type specified.", "type");
			@namespace = type.Namespace;
		}

		public override string ToString()
		{
			return "Collection(" + collectionType.Name + "[])";
		}

		public Type Type
		{
			get { return collectionType; }
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
			get { return TypeScriptTypeCode.Collection; }
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