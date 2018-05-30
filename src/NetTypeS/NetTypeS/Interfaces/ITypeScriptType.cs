using System;

namespace NetTypeS.Interfaces
{
    /// <summary>
    /// TypeScript type interface
    /// </summary>
    public interface ITypeScriptType
    {
        /// <summary>
        /// Type name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Type namespace
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// Type full name
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Type code
        /// </summary>
        TypeScriptTypeCode Code { get; }

        /// <summary>
        /// Is type nullable or not
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// CLR custom type attributes
        /// </summary>
        Attribute[] CustomAttributes { get; }
    }
}