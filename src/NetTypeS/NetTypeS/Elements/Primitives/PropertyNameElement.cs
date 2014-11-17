using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript code property name element. Element formats property name with name formatter.
	/// </summary>
	public class PropertyNameElement : EmptyElement
	{
		private readonly string propertyName;

		/// <summary>
		/// Creates a new property name element
		/// </summary>
		/// <param name="propertyName">Property name</param>
		public PropertyNameElement(string propertyName)
		{
			this.propertyName = propertyName;
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			context.Builder.Append(context.Formatter.FormatPropertyName(propertyName));
		}
	}
}