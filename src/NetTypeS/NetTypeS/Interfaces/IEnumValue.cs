using System;

namespace NetTypeS.Interfaces
{
    /// <summary>
    /// Type script enumerable value type
    /// </summary>
    public interface IEnumValue
    {
        long ValueAsInt64();
        string Name { get; }
        object Value { get; }
        bool HasValue { get; }
        Attribute[] CustomAttributes { get; }
    }
}