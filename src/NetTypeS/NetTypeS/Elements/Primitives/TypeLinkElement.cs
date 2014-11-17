using System;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript type link element. Creates full type reference in the current module.
	/// </summary>
	public class TypeLinkElement : EmptyElement
	{
		private readonly Type type;
		private readonly string moduleName;
		private readonly string typeName;

		/// <summary>
		/// Creates a new type link code element.
		/// </summary>
		/// <param name="moduleName">Type module name</param>
		/// <param name="typeName">Type name</param>
		public TypeLinkElement(string moduleName, string typeName)
			: this(null, moduleName, typeName)
		{
		}

		/// <summary>
		/// Creates a new type link code element.
		/// </summary>
		/// <param name="type">Type</param>
		public TypeLinkElement(Type type) : this(type, null, null)
		{
		}

		private TypeLinkElement(Type type, string moduleName, string typeName)
		{
			this.type = type;
			this.moduleName = moduleName;
			this.typeName = typeName;
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			if (type != null)
			{
				var me = context.TypeElementBuilder.GetTypeModuleElement(type);
				if (me != null)
				{
					me.Generate(context);
					context.Builder.Append(".");
				}
				context.TypeElementBuilder.GetTypeNameElement(type).Generate(context);
			}
			else
			{
				if (!string.IsNullOrEmpty(moduleName))
				{
					context.Builder.Append(context.Formatter.FormatModuleName(moduleName));
					context.Builder.Append(".");
				}
				context.Builder.Append(typeName);
			}
		}
	}
}