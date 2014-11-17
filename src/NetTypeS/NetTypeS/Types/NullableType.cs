using System;
using NetTypeS.Interfaces;

namespace NetTypeS.Types
{
	internal class NullableType : INullableType
	{
		private readonly Type underlyingType;

		public NullableType(Type underlyingType)
		{
			this.underlyingType = underlyingType;
		}

		public override string ToString()
		{
			return "Nullable(" + underlyingType.Name + ")";
		}

		public Type UnderlyingType
		{
			get { return underlyingType; }
		}

		public string Name
		{
			get { return ""; }
		}

		public string Namespace
		{
			get { return ""; }
		}

		public string FullName
		{
			get { return ""; }
		}

		public TypeScriptTypeCode Code
		{
			get { return TypeScriptTypeCode.Nullable; }
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