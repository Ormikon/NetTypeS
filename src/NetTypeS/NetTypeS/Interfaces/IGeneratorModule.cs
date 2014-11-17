using System.Collections.Generic;

namespace NetTypeS.Interfaces
{
	/// <summary>
	/// TypeScript generator module.
	/// </summary>
	public interface IGeneratorModule
	{
		/// <summary>
		/// Gets current generator.
		/// </summary>
		IGenerator Generator { get; }

		/// <summary>
		/// Parent module if exists.
		/// </summary>
		IGeneratorModule Parent { get; }

		/// <summary>
		/// Collection of the module elements generated dynamically during generation.
		/// </summary>
		ICollection<IDynamicElement> DynamicElements { get; }

		/// <summary>
		/// Module name. Empty for root module.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Module full name.
		/// </summary>
		string FullName { get; }

		/// <summary>
		/// If module is declaration
		/// </summary>
		bool Declaration { get; }

		/// <summary>
		/// If module should be exported
		/// </summary>
		bool Export { get; }
	}
}