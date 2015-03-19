using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using NetTypeS.Delegates;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS
{
	public class GeneratorSettings : ICloneable, IGeneratorSettings
	{
		private readonly Collection<Assembly> inheritedTypeAssemblies;

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
			inheritedTypeAssemblies = new Collection<Assembly>();
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

		#region Inherited types

		public Collection<Assembly> InheritedTypeAssemblies
		{
			get { return inheritedTypeAssemblies; }
		}

		IReadOnlyCollection<Assembly> IGeneratorSettings.InheritedTypeAssemblies
		{
			get { return inheritedTypeAssemblies; }
		}

		public bool IncludeInheritedTypes { get; set; }

        public bool GenerateNumberTypeForDictionaryKeys { get; set; }

		#endregion

		/// <summary>
		/// Clone settings
		/// </summary>
		/// <returns>Cloned settings</returns>
		public GeneratorSettings Clone()
		{
			var result = new GeneratorSettings
			       {
				       Format = Format.Clone(),
				       TypeNameResolver = TypeNameResolver ?? Default.TypeNameResolver,
				       PropertyNameResolver = PropertyNameResolver ?? Default.PropertyNameResolver,
				       ModuleNameFormatter = ModuleNameFormatter ?? Default.ModuleNameFormatter,
				       InterfaceNameFormatter = InterfaceNameFormatter ?? Default.InterfaceNameFormatter,
				       EnumNameFormatter = EnumNameFormatter ?? Default.EnumNameFormatter,
				       PropertyNameFormatter = PropertyNameFormatter ?? Default.PropertyNameFormatter,
				       PropertyFilter = PropertyFilter ?? Default.PropertyFilter,
				       AllPropertiesAreOptional = AllPropertiesAreOptional,
					   IncludeInheritedTypes = IncludeInheritedTypes,
                       GenerateNumberTypeForDictionaryKeys = GenerateNumberTypeForDictionaryKeys
			       };
			foreach (var assembly in InheritedTypeAssemblies)
			{
				result.InheritedTypeAssemblies.Add(assembly);
			}

			return result;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}