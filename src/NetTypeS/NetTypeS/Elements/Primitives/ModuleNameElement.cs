using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript module name element. Element formats module name with name formatter.
	/// </summary>
	public class ModuleNameElement : EmptyElement
	{
		private readonly string moduleName;

		/// <summary>
		/// Creates a new module element
		/// </summary>
		/// <param name="moduleName">Module name</param>
		public ModuleNameElement(string moduleName)
		{
			this.moduleName = moduleName;
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			context.Builder.Append(context.Formatter.FormatModuleName(moduleName));
		}
	}
}