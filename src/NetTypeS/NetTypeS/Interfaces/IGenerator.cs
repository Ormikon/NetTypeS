using NetTypeS.Types;
using System.Collections.Generic;

namespace NetTypeS.Interfaces
{
	/// <summary>
	/// TypeScript code generator interface
	/// </summary>
	public interface IGenerator
	{
		/// <summary>
		/// Creates a new module it it is not exist.
		/// </summary>
		/// <param name="moduleName">Module name.</param>
		/// <param name="decalration">If module should be declared.</param>
		/// <param name="export">If module should be exported.</param>
		/// <returns>Found or created module.</returns>
		IGeneratorModule GetModule(string moduleName, bool decalration = false, bool export = false);

        /// <summary>
        /// Generates TypeScript with namespace hierarchy. You can optionally specify particular namespaces to generate with exportedNamespaces parameter.
        /// </summary>
        /// <param name="exportedNamespaces">Namespaces for generation.</param>
        /// <returns>Generated string.</returns>
        string GenerateNamespaces(params string[] exportedNamespaces);

        /// <summary>
        /// Generates single ES6 TypeScript module
        /// </summary>
        /// <param name="exportedModule">Modules to generate.</param>
        /// <returns>Generated string.</returns>
        string GenerateModule(string exportedModule);

        /// <summary>
        /// Contains information about custom names registered in the generator.
        /// </summary>
        ICustomTypeNameHolder CustomTypeNameHolder { get; }

		/// <summary>
		/// Type collector. Collects information about types added for generation.
		/// </summary>
		ITypeCollector TypeCollector { get; }

		/// <summary>
		/// Inherited type lookup interface.
		/// </summary>
		IInheritedTypeSpy InheritedTypeSpy { get; }

		/// <summary>
		/// Generator settings.
		/// </summary>
		IGeneratorSettings Settings { get; }

		/// <summary>
		/// TypeScript file references.
		/// </summary>
		ICollection<string> References { get; }
    }
}