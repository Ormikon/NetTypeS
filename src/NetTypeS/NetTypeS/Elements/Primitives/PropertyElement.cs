using System;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript code property element. Creates formatted interface property element.
	/// </summary>
	public class PropertyElement : EmptyElement
	{
		private readonly ITypeProperty typeProperty;
		private readonly PropertyNameElement propertyName;
		private readonly bool? required;
		private readonly TypeLinkElement typeLink;

		/// <summary>
		/// Creates a new property code element
		/// </summary>
		/// <param name="propertyName">Property name</param>
		/// <param name="type">Property type</param>
		public PropertyElement(string propertyName, Type type)
			: this(new PropertyNameElement(propertyName), new TypeLinkElement(type))
		{
		}

		/// <summary>
		/// Creates a new property code element
		/// </summary>
		/// <param name="propertyName">Property name</param>
		/// <param name="type">Property type</param>
		/// <param name="required">Is required</param>
		public PropertyElement(string propertyName, Type type, bool required)
			: this(new PropertyNameElement(propertyName), new TypeLinkElement(type), required)
		{
		}

		/// <summary>
		/// Creates a new property code element
		/// </summary>
		/// <param name="propertyName">Property name code element</param>
		/// <param name="typeLink">Property type link element</param>
		public PropertyElement(PropertyNameElement propertyName, TypeLinkElement typeLink)
			: this(propertyName, typeLink, true)
		{
		}

		/// <summary>
		/// Creates a new property code element
		/// </summary>
		/// <param name="propertyName">Property name code element</param>
		/// <param name="typeLink">Property type link element</param>
		/// <param name="required">Is required</param>
		public PropertyElement(PropertyNameElement propertyName, TypeLinkElement typeLink, bool required)
			: this(null, propertyName, typeLink, required)
		{
		}

		/// <summary>
		/// Creates a new property code element
		/// </summary>
		/// <param name="typeProperty">Property</param>
		public PropertyElement(ITypeProperty typeProperty) : this(typeProperty, null, null, null)
		{
		}

		/// <summary>
		/// Creates a new property code element
		/// </summary>
		/// <param name="typeProperty">Property</param>
		/// <param name="required">Is required</param>
		public PropertyElement(ITypeProperty typeProperty, bool required)
			: this(typeProperty, null, null, required)
		{
		}

		private PropertyElement(ITypeProperty typeProperty, PropertyNameElement propertyName, TypeLinkElement typeLink,
			bool? required)
		{
			this.typeProperty = typeProperty;
			this.propertyName = propertyName;
			this.required = required;
			this.typeLink = typeLink;
		}

		public override void Generate(IGeneratorModuleContext context)
		{
			if (typeProperty != null)
			{
				var pn = new PropertyNameElement(context.NameResolver.GetPropertyName(typeProperty));
				var tl = new TypeLinkElement(typeProperty.Type);
				pn.Generate(context);
				if (required != null)
				{
					if (!required.Value)
						context.Builder.Append("?");
				}
				else if (context.TypeInfo.IsNullable(typeProperty.Type))
				{
					context.Builder.Append("?");
				}
				context.Builder.Append(": ");
				tl.Generate(context);
			}
			else
			{
				propertyName.Generate(context);
				if (required != null && !required.Value)
				{
					context.Builder.Append("?");
				}
				context.Builder.Append(": ");
				typeLink.Generate(context);
			}
		}
	}
}