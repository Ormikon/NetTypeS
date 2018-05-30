using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript enumeration value name element. Element formats enumeration value name with name formatter.
    /// </summary>
    public class EnumValueNameElement : EmptyElement
    {
        private readonly string _enumValueName;

        public EnumValueNameElement(string enumValueName)
        {
            _enumValueName = enumValueName;
        }

        public override void Generate(IGeneratorModuleContext context)
        {
            context.Builder.Append(context.Formatter.FormatEnumValueName(_enumValueName));
        }
    }
}
