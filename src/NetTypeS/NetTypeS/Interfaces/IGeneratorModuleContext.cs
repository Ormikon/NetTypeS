namespace NetTypeS.Interfaces
{
    /// <summary>
    /// Generator's module context
    /// </summary>
    public interface IGeneratorModuleContext
    {
        /// <summary>
        /// Script builder
        /// </summary>
        IScriptBuilder Builder { get; }

        /// <summary>
        /// Element filter
        /// </summary>
        IElementFilter Filter { get; }

        /// <summary>
        /// Element name formatter
        /// </summary>
        IElementNameFormatter Formatter { get; }

        /// <summary>
        /// Element custom name resolver
        /// </summary>
        IElementNameResolver NameResolver { get; }

        /// <summary>
        /// Additional type info provider
        /// </summary>
        ITypeInfo TypeInfo { get; }

        /// <summary>
        /// Helper for building type element names and type links
        /// </summary>
        ITypeElementBuilder TypeElementBuilder { get; }

        /// <summary>
        /// Is all the interface properties should be optional
        /// </summary>
        bool AllPropertiesAreOptional { get; }
    }
}