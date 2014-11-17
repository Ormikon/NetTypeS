using System.Collections.Generic;
using System.Linq;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript code block element {}
	/// </summary>
	public class BlockElement : FixedElement
	{
		private readonly bool inline;

		/// <summary>
		/// Creates a new code block element
		/// </summary>
		/// <param name="elements">Elements in the block</param>
		/// <param name="inline">Inline</param>
		public BlockElement(IEnumerable<ITypeScriptElement> elements, bool inline = false)
			: base(elements == null ? null : elements.ToArray())
		{
			this.inline = inline;
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			// Custom formatting for the block element (added indentation)
			context.Builder.Append("{");
			if (Count > 0)
			{
				if (!inline)
				{
					context.Builder.AppendLine();
					using (context.Builder.Indent())
					{
						foreach (var el in this)
						{
							el.Generate(context);
							context.Builder.AppendLine();
						}
					}
				}
				else
				{
					foreach (var el in this)
					{
						el.Generate(context);
					}
				}
			}
			context.Builder.Append("}");
		}
	}
}