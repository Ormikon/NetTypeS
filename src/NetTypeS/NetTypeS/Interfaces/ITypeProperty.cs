using System;
using System.Collections.Generic;

namespace NetTypeS.Interfaces
{
    /// <summary>
    /// TypeScript interface property
    /// </summary>
    public interface ITypeProperty
    {
        IReadOnlyCollection<Attribute> GetCustomAttributes();
        IEnumerable<Attribute> GetCustomAttributes(Type attributeType);
        IEnumerable<T> GetCustomAttributes<T>() where T : Attribute;
        string Name { get; }
        bool CanWrite { get; }
        bool CanRead { get; }
        Type Type { get; }
    }
}