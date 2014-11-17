using NetTypeS.Interfaces;

namespace NetTypeS
{
	internal class GeneratorModuleContext : IGeneratorModuleContext
	{
		private readonly IScriptBuilder builder;
		private readonly IElementFilter filter;
		private readonly IElementNameFormatter formatter;
		private readonly ITypeElementBuilder typeElementBuilder;
		private readonly IElementNameResolver nameResolver;
		private readonly ITypeInfo typeInfo;
		private readonly bool allPropertiesAreOptional;

		public GeneratorModuleContext(string moduleName, IScriptBuilder builder, ITypeCollector collector,
			ICustomTypeNameHolder customNamesHolder, IGeneratorSettings settings)
		{
			this.builder = builder;
			filter = new ElementFilter(settings);
			formatter = new ElementNameFormatter(settings);
			typeElementBuilder = new TypeElementBuilder(moduleName, collector, customNamesHolder, settings);
			nameResolver = new ElementNameResolver(collector, customNamesHolder, settings);
			typeInfo = new TypeInfo(collector);
			allPropertiesAreOptional = settings.AllPropertiesAreOptional;
		}

		public IScriptBuilder Builder
		{
			get { return builder; }
		}

		public IElementFilter Filter
		{
			get { return filter; }
		}

		public IElementNameFormatter Formatter
		{
			get { return formatter; }
		}

		public IElementNameResolver NameResolver
		{
			get { return nameResolver; }
		}

		public ITypeInfo TypeInfo
		{
			get { return typeInfo; }
		}

		public ITypeElementBuilder TypeElementBuilder
		{
			get { return typeElementBuilder; }
		}

		public bool AllPropertiesAreOptional
		{
			get { return allPropertiesAreOptional; }
		}
	}
}