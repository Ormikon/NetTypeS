using System;

namespace NetTypeS.Interfaces
{
	public interface INullableType : ITypeScriptType
	{
		Type UnderlyingType { get; }
	}
}