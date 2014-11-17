using System;

namespace NetTypeS.Interfaces
{
	/// <summary>
	/// Type script enumerable value type
	/// </summary>
	public interface IEnumValue
	{
		int ValueAsInt32();
		string Name { get; }
		object Value { get; }
		bool HasValue { get; }
		Attribute[] CustomAttributes { get; }
	}
}