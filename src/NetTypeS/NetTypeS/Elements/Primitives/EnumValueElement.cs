using System.Globalization;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript enumeration value code element. Creates a formatted enumerable value.
    /// </summary>
    public class EnumValueElement : EmptyElement
    {
        private readonly IEnumValue _enumValue;
        private readonly string _enumValueName;
        private readonly int? _enumValueInt;

        /// <summary>
        /// Creates a new enumerable value element
        /// </summary>
        /// <param name="enumValueName">Value name</param>
        /// <param name="enumValue">Optional value</param>
        public EnumValueElement(string enumValueName, int? enumValue = null)
            : this(null, enumValueName, enumValue)
        {
        }

        /// <summary>
        /// Creates a new enumerable value element
        /// </summary>
        /// <param name="enumValue">Enum value</param>
        public EnumValueElement(IEnumValue enumValue) : this(enumValue, null, null)
        {
        }

        private EnumValueElement(IEnumValue enumValue, string enumValueName, int? enumValueInt)
        {
            _enumValue = enumValue;
            _enumValueName = enumValueName;
            _enumValueInt = enumValueInt;
        }

        public override void Generate(IGeneratorModuleContext context)
        {
            if (_enumValue != null)
            {
                var name = context.NameResolver.GetEnumValueName(_enumValue);
                context.Builder.Append(name);
                context.Builder.Append(" = ");
                context.Builder.Append(_enumValue.ValueAsInt64().ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                context.Builder.Append(_enumValueName);
                if (_enumValueInt != null)
                {
                    context.Builder.Append(" = ");
                    context.Builder.Append(_enumValueInt.Value.ToString(CultureInfo.InvariantCulture));
                }
            }
        }
    }
}
