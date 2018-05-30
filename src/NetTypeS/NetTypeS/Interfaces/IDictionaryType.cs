using System;

namespace NetTypeS.Interfaces
{
    /// <summary>
    /// TypeScript collection type
    /// </summary>
    public interface IDictionaryType : ITypeScriptType
    {
        Type KeyType { get; }
        Type ValueType { get; }
    }
}