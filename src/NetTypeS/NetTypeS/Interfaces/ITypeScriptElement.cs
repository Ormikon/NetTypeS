using System.Collections.Generic;

namespace NetTypeS.Interfaces
{
    /// <summary>
    /// Basic TypeScript code element interface
    /// </summary>
    public interface ITypeScriptElement : ICollection<ITypeScriptElement>
    {
        /// <summary>
        /// Element generation method
        /// </summary>
        /// <param name="context">Generator module context</param>
        void Generate(IGeneratorModuleContext context);
    }
}