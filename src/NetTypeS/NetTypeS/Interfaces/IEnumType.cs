using System;
using System.Collections.Generic;

namespace NetTypeS.Interfaces
{
	/// <summary>
	/// TypeScript enumerable type
	/// </summary>
	public interface IEnumType : ITypeScriptType
	{
		Type Type { get; }
		IReadOnlyList<IEnumValue> Values { get; }
	}
}