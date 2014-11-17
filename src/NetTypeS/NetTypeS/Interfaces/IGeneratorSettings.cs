using NetTypeS.Delegates;

namespace NetTypeS.Interfaces
{
	public interface IGeneratorSettings
	{
		IGeneratorFormatSettings Format { get; }
		CustomTypeNameResolver TypeNameResolver { get; }
		CustomPropertyNameResolver PropertyNameResolver { get; }
		CustomNameFormatter ModuleNameFormatter { get; }
		CustomNameFormatter InterfaceNameFormatter { get; }
		CustomNameFormatter EnumNameFormatter { get; }
		CustomNameFormatter PropertyNameFormatter { get; }
		CustomPropertyFilter PropertyFilter { get; }
		bool AllPropertiesAreOptional { get; }
	}
}