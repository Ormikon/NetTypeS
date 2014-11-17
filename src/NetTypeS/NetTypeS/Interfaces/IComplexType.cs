using System;
using System.Collections.Generic;

namespace NetTypeS.Interfaces
{
	/// <summary>
	/// TypeScript complex type (interface)
	/// </summary>
	public interface IComplexType : ITypeScriptType
	{
		Type Type { get; }
		bool IsValueType { get; }
		bool IsGenerated { get; }
		bool IsGeneric { get; }
		bool IsGenericDefinition { get; }
		Type GenericType { get; }
		Type[] GenericArguments { get; }
		IReadOnlyList<ITypeProperty> Properties { get; }
	}
}