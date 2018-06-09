using System;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript type link element. Creates full type reference in the current module.
    /// </summary>
    public class TypeLinkElement : EmptyElement
    {
        private readonly Type _type;
        private readonly string _moduleName;
        private readonly string _typeName;

        /// <summary>
        /// Creates a new type link code element.
        /// </summary>
        /// <param name="moduleName">Type module name</param>
        /// <param name="typeName">Type name</param>
        public TypeLinkElement(string moduleName, string typeName)
            : this(null, moduleName, typeName)
        {
        }

        /// <summary>
        /// Creates a new type link code element.
        /// </summary>
        /// <param name="type">Type</param>
        public TypeLinkElement(Type type) : this(type, null, null)
        {
        }

        private TypeLinkElement(Type type, string moduleName, string typeName)
        {
            _type = type;
            _moduleName = moduleName;
            _typeName = typeName;
        }

        public override void Generate(IGeneratorModuleContext context)
        {
            if (_type != null)
            {
                var me = context.TypeElementBuilder.GetTypeModuleElement(_type);
                if (me != null)
                {
                    me.Generate(context);
                    context.Builder.Append(".");
                }
                context.TypeElementBuilder.GetTypeNameElement(_type).Generate(context);
            }
            else
            {
                if (!string.IsNullOrEmpty(_moduleName))
                {
                    context.Builder.Append(context.Formatter.FormatModuleName(_moduleName));
                    context.Builder.Append(".");
                }
                context.Builder.Append(_typeName);
            }
        }
    }
}