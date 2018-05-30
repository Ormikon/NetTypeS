using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript code property name element. Element formats property name with name formatter.
    /// </summary>
    public class PropertyNameElement : EmptyElement
    {
        private readonly string _propertyName;

        /// <summary>
        /// Creates a new property name element
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public PropertyNameElement(string propertyName)
        {
            _propertyName = propertyName;
        }

        public override void Generate(IGeneratorModuleContext context)
        {
            context.Builder.Append(context.Formatter.FormatPropertyName(_propertyName));
        }
    }
}