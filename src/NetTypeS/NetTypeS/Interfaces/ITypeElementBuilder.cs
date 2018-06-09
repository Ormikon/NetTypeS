using System;

namespace NetTypeS.Interfaces
{
    public interface ITypeElementBuilder
    {
        ITypeScriptElement GetTypeNameElement(Type type);
        ITypeScriptElement GetTypeModuleElement(Type type);
    }
}