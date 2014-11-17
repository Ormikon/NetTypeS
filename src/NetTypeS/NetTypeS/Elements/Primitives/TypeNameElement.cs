using System;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript code type name element. Element formats type name with name formatter.
	/// </summary>
	public class TypeNameElement : EmptyElement
	{
		private readonly Type type;
		private readonly string typeName;

		/// <summary>
		/// Creates a new type name element.
		/// </summary>
		/// <param name="typeName">Type name</param>
		public TypeNameElement(string typeName) : this(null, typeName)
		{
		}

		/// <summary>
		/// Creates a new type name element.
		/// </summary>
		/// <param name="type">Type</param>
		public TypeNameElement(Type type) : this(type, null)
		{
		}

		private TypeNameElement(Type type, string typeName)
		{
			this.type = type;
			this.typeName = typeName;
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			if (type != null)
			{
				var el = context.TypeElementBuilder.GetTypeNameElement(type);
				el.Generate(context);
			}
			else
			{
				context.Builder.Append(typeName);
			}
		}
	}
}