using System;
using System.Collections.Generic;

namespace NetTypeS.Interfaces
{
	/// <summary>
	/// Inherited types lookup
	/// </summary>
	public interface IInheritedTypeSpy
	{
		/// <summary>
		/// Returns types inherited from the pointed type.
		/// </summary>
		/// <param name="type">Type to look for inherited</param>
		/// <returns>Inherited types</returns>
		IEnumerable<Type> GetInheritedTypes(Type type);
	}
}