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
		private readonly TypeNameElement name;
		private readonly IReadOnlyCollection<TypeLinkElement> extends;
		private readonly IReadOnlyCollection<ITypeProperty> properties;
		private readonly ITypeScriptElement body;
		private readonly bool export;
		private readonly bool declaration;

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
			: this(name, extends, (IEnumerable<ITypeScriptElement>) members, declaration, export)
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
			this.name = name;
			this.properties = properties == null ? null : properties.ToArray();
			body = members == null ? null : new BlockElement(AppendSemicolon(members));
			this.extends = extends == null ? null : extends.ToArray();
			this.export = export;
			this.declaration = declaration;
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
			if (declaration)
				b.Append("declare ");
			else if (export)
				b.Append("export ");
			b.Append("interface ");
			name.Generate(context);
			b.Append(" ");
			if (extends != null && extends.Count > 0)
			{
				b.Append("extends ");
				int extIdx = 0;
				foreach (var extEl in extends)
				{
					extEl.Generate(context);
					extIdx++;
					if (extIdx != extends.Count)
						b.Append(", ");
				}
				b.Append(" ");
			}
			var bd = properties == null
				? body
				: new BlockElement(AppendSemicolon(properties.Where(p => context.Filter.IsPropertyIncluded(p))
					.Select(p => context.AllPropertiesAreOptional ? new PropertyElement(p, false) : new PropertyElement(p))));
			bd.Generate(context);
		}
	}
}