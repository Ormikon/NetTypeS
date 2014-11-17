using System;

namespace NetTypeS.Interfaces
{
	/// <summary>
	/// Special interface to register and resolve custom names for types
	/// </summary>
	public interface ICustomTypeNameHolder
	{
		void RegisterNameFor(Type type, string name);
		string GetNameFor(Type type);
	}
}