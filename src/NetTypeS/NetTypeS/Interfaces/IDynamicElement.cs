using System.Collections.Generic;

namespace NetTypeS.Interfaces
{
    /// <summary>
    /// Dynamic element for Generator's module. Dynamically provides TypeScript elements for the module.
    /// </summary>
    public interface IDynamicElement
    {
        /// <summary>
        /// Gets collection of the elements for the current moment and the current module.
        /// </summary>
        /// <returns>TypeScrip elements.</returns>
        IEnumerable<ITypeScriptElement> GetElements();
    }
}