using System.Collections.Generic;
using System.Reflection;
using NetTypeS.Delegates;

namespace NetTypeS.Interfaces
{
    /// <summary>
    /// TypeScript generator settings
    /// </summary>
    public interface IGeneratorSettings
    {
        /// <summary>
        /// Generator formatting settings
        /// </summary>
        IGeneratorFormatSettings Format { get; }

        /// <summary>
        /// Type name resolution delegate
        /// </summary>
        CustomTypeNameResolver TypeNameResolver { get; }

        /// <summary>
        /// Property name resolution delegate
        /// </summary>
        CustomPropertyNameResolver PropertyNameResolver { get; }

        /// <summary>
        /// Module name format delegate
        /// </summary>
        CustomNameFormatter ModuleNameFormatter { get; }

        /// <summary>
        /// Interface name format delegate
        /// </summary>
        CustomNameFormatter InterfaceNameFormatter { get; }

        /// <summary>
        /// Enumerable name format delegate
        /// </summary>
        CustomNameFormatter EnumNameFormatter { get; }

        /// <summary>
        /// Properti name format delegate
        /// </summary>
        CustomNameFormatter PropertyNameFormatter { get; }

        /// <summary>
        /// Property filter delegate
        /// </summary>
        CustomPropertyFilter PropertyFilter { get; }

        /// <summary>
        /// Gets if all the interface properties will be optional in the generated interface
        /// </summary>
        bool AllPropertiesAreOptional { get; }

        /// <summary>
        /// Gets if all optional propertiest should be generated as nullable or as optional (with undefined)
        /// </summary>
        OptionalPropertiesStyle OptionalPropertiesStyle { get; }

        /// <summary>
        /// Collection of assemblies to look for inherited types.
        /// </summary>
        IReadOnlyCollection<Assembly> InheritedTypeAssemblies { get; }

        /// <summary>
        /// Gets if including of inherited types is enabled
        /// </summary>
        bool IncludeInheritedTypes { get; }

        /// <summary>
        /// Gets if number-like types in dictionaries keys should be threated as TS number type, instead of string
        /// </summary>
        bool GenerateNumberTypeForDictionaryKeys { get; }

        /// <summary>
        /// Gets if add GET query parameters as one object insted of using string parameters
        /// </summary>
        bool QueryParametersAsObject { get; }
    }
}