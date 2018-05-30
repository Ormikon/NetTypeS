using System;

namespace NetTypeS.Interfaces
{
    /// <summary>
    /// Elements name resolving interface
    /// </summary>
    public interface IElementNameResolver
    {
        string GetTypeName(Type type);
        string GetPropertyName(ITypeProperty typeProperty);
        string GetEnumValueName(IEnumValue enumValue);
    }
}