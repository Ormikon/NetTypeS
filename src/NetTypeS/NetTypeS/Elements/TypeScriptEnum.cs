using System.Collections.Generic;
using System.Linq;
using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements
{
	/// <summary>
	/// TypeScript code enumerable element
	/// </summary>
	public class TypeScriptEnum : FixedElement
	{
		private readonly TypeNameElement name;
		private readonly ITypeScriptElement valuesBody;
		private readonly bool export;
		private readonly bool declaration;

		public TypeScriptEnum(IEnumType enumType, bool declaration = false, bool export = false)
			: this(new TypeNameElement(enumType.Type), enumType.Values, declaration, export)
		{
		}

		public TypeScriptEnum(TypeNameElement name, IEnumerable<string> values, bool declaration = false, bool export = false)
			: this(name, values == null ? null : values.Select(v => new EnumValueElement(v)), declaration, export)
		{
		}

		public TypeScriptEnum(TypeNameElement name, IEnumerable<IEnumValue> values, bool declaration = false,
			bool export = false)
			: this(name, values == null ? null : values.Select(v => new EnumValueElement(v)), declaration, export)
		{
		}

		public TypeScriptEnum(TypeNameElement name, IEnumerable<EnumValueElement> values, bool declaration = false,
			bool export = false)
			: this(name, (IEnumerable<ITypeScriptElement>) values, declaration, export)
		{
		}

		public TypeScriptEnum(TypeNameElement name, IEnumerable<ITypeScriptElement> values, bool declaration = false,
			bool export = false)
		{
			this.name = name;
			valuesBody = new BlockElement(AppendCommas(values));
			this.declaration = declaration;
			this.export = export;
		}

		private static IEnumerable<ITypeScriptElement> AppendCommas(IEnumerable<ITypeScriptElement> values)
		{
			if (values == null)
				yield break;
			var va = values.ToArray();
			for (int i = 0; i < va.Length; i++)
			{
				if (i != va.Length - 1)
					yield return new FixedElement(va[i], new TextElement(","));
				else
					yield return va[i];
			}
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			var b = context.Builder;
			if (declaration)
				b.Append("declare ");
			else if (export)
				b.Append("export ");
			b.Append("enum ");
			name.Generate(context);
			b.Append(" ");
			valuesBody.Generate(context);
		}
	}
}