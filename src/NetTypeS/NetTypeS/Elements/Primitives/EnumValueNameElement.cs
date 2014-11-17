using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript enumeration value name element. Element formats enumeration value name with name formatter.
	/// </summary>
	public class EnumValueNameElement : EmptyElement
	{
		private readonly string enumValueName;

		public EnumValueNameElement(string enumValueName)
		{
			this.enumValueName = enumValueName;
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			context.Builder.Append(context.Formatter.FormatEnumValueName(enumValueName));
		}
	}
}