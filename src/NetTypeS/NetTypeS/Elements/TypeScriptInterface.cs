using System.Collections.Generic;
using System.Linq;
using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements
{
    /// <summary>
    /// TypeScript code interface element
    /// </summary>
    public class TypeScriptInterface : EmptyElement
    {
        private readonly TypeNameElement _name;
        private readonly IReadOnlyCollection<TypeLinkElement> _extends;
        private readonly IReadOnlyCollection<ITypeProperty> _properties;
        private readonly ITypeScriptElement _body;
        private readonly bool _export;
        private readonly bool _declaration;

        public TypeScriptInterface(IComplexType type, bool declaration = false, bool export = false)
            : this(new TypeNameElement(type.Type), GetExtensionsIfInterface(type), type.Properties, declaration, export)
        {
        }

        public TypeScriptInterface(TypeNameElement name, IEnumerable<PropertyElement> members,
            bool declaration = false, bool export = false)
            : this(name, null, members, declaration, export)
        {
        }

        public TypeScriptInterface(TypeNameElement name, IEnumerable<ITypeProperty> properties,
            bool declaration = false, bool export = false)
            : this(name, null, properties, null, declaration, export)
        {
        }

        public TypeScriptInterface(TypeNameElement name, IEnumerable<TypeLinkElement> extends,
            IEnumerable<PropertyElement> members,
            bool declaration = false, bool export = false)
            : this(name, extends, (IEnumerable<ITypeScriptElement>)members, declaration, export)
        {
        }

        public TypeScriptInterface(TypeNameElement name, IEnumerable<ITypeScriptElement> members,
            bool declaration = false, bool export = false)
            : this(name, null, members, declaration, export)
        {
        }

        public TypeScriptInterface(TypeNameElement name, IEnumerable<TypeLinkElement> extends,
            IEnumerable<ITypeProperty> properties,
            bool declaration = false, bool export = false)
            : this(name, extends, properties, null, declaration, export)
        {
        }

        public TypeScriptInterface(TypeNameElement name, IEnumerable<TypeLinkElement> extends,
            IEnumerable<ITypeScriptElement> members,
            bool declaration = false, bool export = false)
            : this(name, extends, null, members, declaration, export)
        {
        }

        public TypeScriptInterface(TypeNameElement name, IEnumerable<TypeLinkElement> extends,
            IEnumerable<ITypeProperty> properties,
            IEnumerable<ITypeScriptElement> members, bool declaration = false, bool export = false)
        {
            _name = name;
            _properties = properties?.ToArray();
            _body = members == null ? null : new BlockElement(AppendSemicolon(members));
            _extends = extends?.ToArray();
            _export = export;
            _declaration = declaration;
        }

        private static IEnumerable<TypeLinkElement> GetExtensionsIfInterface(IComplexType complexType)
        {
            if (!complexType.IsInterface || complexType.Interfaces.Length == 0)
                return null;
            return complexType.Interfaces.Select(i => new TypeLinkElement(i));
        }

        private static IEnumerable<ITypeScriptElement> AppendSemicolon(IEnumerable<ITypeScriptElement> members)
        {
            return members == null ? null : members.Select(m => new FixedElement(m, new TextElement(";")));
        }

        public override void Generate(IGeneratorModuleContext context)
        {
            var b = context.Builder;
            if (_declaration)
                b.Append("declare ");
            else if (_export)
                b.Append("export ");
            b.Append("interface ");
            _name.Generate(context);
            b.Append(" ");
            if (_extends != null && _extends.Count > 0)
            {
                b.Append("extends ");
                int extIdx = 0;
                foreach (var extEl in _extends)
                {
                    extEl.Generate(context);
                    extIdx++;
                    if (extIdx != _extends.Count)
                        b.Append(", ");
                }
                b.Append(" ");
            }
            var bd = _properties == null
                ? _body
                : new BlockElement(AppendSemicolon(_properties.Where(p => context.Filter.IsPropertyIncluded(p))
                    .Select(p => context.AllPropertiesAreOptional ? new PropertyElement(p, false) : new PropertyElement(p))));
            bd.Generate(context);
        }
    }
}