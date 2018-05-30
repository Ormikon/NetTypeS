using System.Collections.Generic;
using System.Linq;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript code multiline text element. Formats multiline text with current indentation.
    /// </summary>
    public class MultilineTextElement : FixedElement
    {
        /// <summary>
        /// Creates a new multiline text element
        /// </summary>
        /// <param name="text">Text</param>
        public MultilineTextElement(string text)
            : base(GetElements(text))
        {
        }

        private static IEnumerable<ITypeScriptElement> GetElements(string text)
        {
            if (text == null)
                text = "";
            var lines = text.Split('\n').Select(l => l.TrimEnd('\r')).ToArray();
            return GetElementsForLines(lines).ToArray();
        }

        private static IEnumerable<ITypeScriptElement> GetElementsForLines(string[] lines)
        {
            for (var i = 0; i < lines.Length; i++)
            {
                if (i != 0)
                    yield return NewLineElement.Instance;
                yield return new TextElement(lines[i]);
            }
        }
    }
}
