using System;
using NetTypeS.Delegates;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS
{
	public class GeneratorSettings : ICloneable, IGeneratorSettings
	{
		public static class Default
		{
			public static CustomTypeNameResolver TypeNameResolver =
				type => type.Name;

			public static CustomPropertyNameResolver PropertyNameResolver =
				prop => prop.Name;

			public static CustomNameFormatter ModuleNameFormatter =
				name => name;

			public static CustomNameFormatter InterfaceNameFormatter =
				name => name;

			public static CustomNameFormatter EnumNameFormatter =
				name => name;

			public static CustomNameFormatter PropertyNameFormatter =
				name => StringUtils.ToCamelCase(name);

			public static CustomPropertyFilter PropertyFilter =
				prop => !typeof (Delegate).IsAssignableFrom(prop.Type); //Exclude properties with delegates
		}

		public GeneratorSettings()
		{
			Format = new GeneratorFormatSettings();
		}

		public GeneratorFormatSettings Format { get; private set; }

		IGeneratorFormatSettings IGeneratorSettings.Format
		{
			get { return Format; }
		}

		#region Name resolvers

		public CustomTypeNameResolver TypeNameResolver { get; set; }
		public CustomPropertyNameResolver PropertyNameResolver { get; set; }

		#endregion

		#region Formatters

		public CustomNameFormatter ModuleNameFormatter { get; set; }
		public CustomNameFormatter InterfaceNameFormatter { get; set; }
		public CustomNameFormatter EnumNameFormatter { get; set; }
		public CustomNameFormatter PropertyNameFormatter { get; set; }

		#endregion

		#region Filters

		public CustomPropertyFilter PropertyFilter { get; set; }

		public bool AllPropertiesAreOptional { get; set; }

		#endregion

		public GeneratorSettings Clone()
		{
			return new GeneratorSettings
			       {
				       Format = Format.Clone(),
				       TypeNameResolver = TypeNameResolver ?? Default.TypeNameResolver,
				       PropertyNameResolver = PropertyNameResolver ?? Default.PropertyNameResolver,
				       ModuleNameFormatter = ModuleNameFormatter ?? Default.ModuleNameFormatter,
				       InterfaceNameFormatter = InterfaceNameFormatter ?? Default.InterfaceNameFormatter,
				       EnumNameFormatter = EnumNameFormatter ?? Default.EnumNameFormatter,
				       PropertyNameFormatter = PropertyNameFormatter ?? Default.PropertyNameFormatter,
				       PropertyFilter = PropertyFilter ?? Default.PropertyFilter,
				       AllPropertiesAreOptional = AllPropertiesAreOptional
			       };
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}