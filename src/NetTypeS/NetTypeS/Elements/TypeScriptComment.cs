using NetTypeS.Elements.Primitives;

namespace NetTypeS.Elements
{
	/// <summary>
	/// TypeScript code comment
	/// </summary>
	public class TypeScriptComment : FixedElement
	{
		/// <summary>
		/// Creates a new code comment
		/// </summary>
		/// <param name="text">Comment text</param>
		public TypeScriptComment(string text)
			: base(new TextElement("/*"), new MultilineTextElement(text), new TextElement("*/"))
		{
		}
	}
}