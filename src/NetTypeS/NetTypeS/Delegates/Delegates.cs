using System;
using NetTypeS.Interfaces;

namespace NetTypeS.Delegates
{
	public delegate void ModuleBuilder(IGeneratorModule builder);

	public delegate ITypeScriptElement CustomEnumElementBuilder(IEnumType enumType);

	public delegate ITypeScriptElement CustomInterfaceElementBuilder(IComplexType complexType);

	public delegate ITypeScriptElement CustomTypeElementBuilder(Type type);

	public delegate ITypeScriptElement CustomTypeScriptElementBuilder(ITypeScriptType type);

	public delegate string CustomTypeNameResolver(ITypeScriptType type);

	public delegate string CustomPropertyNameResolver(ITypeProperty property);

	public delegate string CustomNameFormatter(string name);

	public delegate bool CustomPropertyFilter(ITypeProperty property);
}