using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript code new line element. Contains instance of the new line element.
	/// </summary>
	public class NewLineElement : EmptyElement
	{
		/// <summary>
		/// NewLine element instance.
		/// </summary>
		public static readonly NewLineElement Instance = new NewLineElement();

		private NewLineElement()
		{
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			context.Builder.AppendLine();
		}
	}
}