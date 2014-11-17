using System.Linq;
using NetTypeS.Interfaces;

namespace NetTypeS
{
	internal class ElementNameFormatter : IElementNameFormatter
	{
		private readonly IGeneratorSettings settings;

		public ElementNameFormatter(IGeneratorSettings settings)
		{
			this.settings = settings;
		}

		public string FormatModuleName(string moduleNme)
		{
			return moduleNme.IndexOf('.') > 0
				? string.Join(".", moduleNme.Split('.').Select(n => settings.ModuleNameFormatter(n)))
				: settings.ModuleNameFormatter(moduleNme);
		}

		public string FormatInterfaceName(string name)
		{
			int moduleIdx = name.LastIndexOf('.');
			if (moduleIdx < 0)
				return settings.InterfaceNameFormatter(name);
			return FormatModuleName(name.Remove(moduleIdx)) + "." +
			       settings.InterfaceNameFormatter(name.Substring(moduleIdx + 1));
		}

		public string FormatEnumName(string name)
		{
			int moduleIdx = name.LastIndexOf('.');
			if (moduleIdx < 0)
				return settings.EnumNameFormatter(name);
			return FormatModuleName(name.Remove(moduleIdx)) + "." +
			       settings.EnumNameFormatter(name.Substring(moduleIdx + 1));
		}

		public string FormatEnumValueName(string name)
		{
			return name;
		}

		public string FormatPropertyName(string name)
		{
			return settings.PropertyNameFormatter(name);
		}
	}
}