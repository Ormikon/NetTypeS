using NetTypeS.Interfaces;

namespace NetTypeS
{
    internal class GeneratorModuleContext : IGeneratorModuleContext
    {
        public GeneratorModuleContext(string moduleName, IScriptBuilder builder, ITypeCollector collector,
            ICustomTypeNameHolder customNamesHolder, IGeneratorSettings settings)
        {
            Builder = builder;
            Filter = new ElementFilter(settings);
            Formatter = new ElementNameFormatter(settings);
            TypeElementBuilder = new TypeElementBuilder(moduleName, collector, customNamesHolder, settings);
            NameResolver = new ElementNameResolver(collector, customNamesHolder, settings);
            TypeInfo = new TypeInfo(collector);
            AllPropertiesAreOptional = settings.AllPropertiesAreOptional;
        }

        public IScriptBuilder Builder { get; }

        public IElementFilter Filter { get; }

        public IElementNameFormatter Formatter { get; }

        public IElementNameResolver NameResolver { get; }

        public ITypeInfo TypeInfo { get; }

        public ITypeElementBuilder TypeElementBuilder { get; }

        public bool AllPropertiesAreOptional { get; }
    }
}