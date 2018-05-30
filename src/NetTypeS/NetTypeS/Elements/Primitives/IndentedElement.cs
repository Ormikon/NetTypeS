using System.Collections.Generic;
using System.Linq;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript code element with instruction to indent inner elements
    /// </summary>
    public class IndentedElement : FixedElement
    {
        /// <summary>
        /// Creates a new indented element.
        /// </summary>
        /// <param name="elements">Elements to indent.</param>
        public IndentedElement(params ITypeScriptElement[] elements)
            : this((IEnumerable<ITypeScriptElement>)elements)
        {
        }

        /// <summary>
        /// Creates a new indented element.
        /// </summary>
        /// <param name="elements">Elements to indent.</param>
        public IndentedElement(IEnumerable<ITypeScriptElement> elements)
            : base(elements?.ToArray())
        {
        }

        public override void Generate(IGeneratorModuleContext context)
        {
            // Custom formatting for the block element (added indentation)
            using (context.Builder.Indent())
            {
                foreach (var el in this)
                {
                    el.Generate(context);
                    context.Builder.AppendLine();
                }
            }
        }
    }
}
