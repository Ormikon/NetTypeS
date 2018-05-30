using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// Type script code text element. Can contains any text.
    /// </summary>
    public class TextElement : EmptyElement
    {
        private readonly string _text;

        /// <summary>
        /// Creates a new text element
        /// </summary>
        /// <param name="text">Text</param>
        public TextElement(string text)
        {
            _text = text ?? "";
        }

        public override void Generate(IGeneratorModuleContext context)
        {
            context.Builder.Append(_text);
        }
    }
}