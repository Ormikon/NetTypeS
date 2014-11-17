using System.Globalization;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript enumeration value code element. Creates a formatted enumerable value.
	/// </summary>
	public class EnumValueElement : EmptyElement
	{
		private readonly IEnumValue enumValue;
		private readonly string enumValueName;
		private readonly int? enumValueInt;

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
			this.enumValue = enumValue;
			this.enumValueName = enumValueName;
			this.enumValueInt = enumValueInt;
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			if (enumValue != null)
			{
				var name = context.NameResolver.GetEnumValueName(enumValue);
				context.Builder.Append(name);
				context.Builder.Append(" = ");
				context.Builder.Append(enumValue.ValueAsInt32().ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				context.Builder.Append(enumValueName);
				if (enumValueInt != null)
				{
					context.Builder.Append(" = ");
					context.Builder.Append(enumValueInt.Value.ToString(CultureInfo.InvariantCulture));
				}
			}
		}
	}
}