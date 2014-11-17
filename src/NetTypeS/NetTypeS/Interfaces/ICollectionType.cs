using System;

namespace NetTypeS.Interfaces
{
	/// <summary>
	/// TypeScript collection type
	/// </summary>
	public interface ICollectionType : ITypeScriptType
	{
		Type Type { get; }
	}
}