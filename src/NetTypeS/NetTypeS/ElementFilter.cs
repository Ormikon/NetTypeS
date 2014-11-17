using NetTypeS.Interfaces;

namespace NetTypeS
{
	internal class ElementFilter : IElementFilter
	{
		private readonly IGeneratorSettings settings;

		public ElementFilter(IGeneratorSettings settings)
		{
			this.settings = settings;
		}

		public bool IsPropertyIncluded(ITypeProperty property)
		{
			return settings.PropertyFilter(property);
		}
	}
}