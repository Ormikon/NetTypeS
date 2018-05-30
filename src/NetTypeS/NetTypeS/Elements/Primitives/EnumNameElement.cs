using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript enumeration name element. Element formats enumeration name with name formatter.
    /// </summary>
    public class EnumNameElement : TypeNameElement
    {
        private readonly string _enumName;

        public EnumNameElement(string enumName)
            : base(enumName)
        {
            _enumName = enumName;
        }

        public override void Generate(IGeneratorModuleContext context)
        {
            context.Builder.Append(context.Formatter.FormatEnumName(_enumName));
        }
    }
}
