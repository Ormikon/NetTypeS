using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript code property element. Creates formatted interface property element.
    /// </summary>
    public class PropertyElement : EmptyElement
    {
        private readonly ITypeProperty _typeProperty;
        private readonly PropertyNameElement _propertyName;
        private readonly bool? _required;
        private readonly TypeLinkElement _typeLink;

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
            _typeProperty = typeProperty;
            _propertyName = propertyName;
            _required = required;
            _typeLink = typeLink;
        }

        public override void Generate(IGeneratorModuleContext context)
        {
            var isRequired = PropertyIsRequired(context);

            if (_typeProperty != null)
            {
                var pn = new PropertyNameElement(context.NameResolver.GetPropertyName(_typeProperty));
                var tl = new TypeLinkElement(_typeProperty.Type);
                pn.Generate(context);
                if (_required != null)
                {
                    if (!isRequired && OptionAsUnderfied(context))
                    {
                        context.Builder.Append("?");
                    }
                }
                else if (!isRequired && OptionAsUnderfied(context))
                {
                    context.Builder.Append("?");
                }
                context.Builder.Append(": ");
                tl.Generate(context);
            }
            else
            {
                _propertyName.Generate(context);
                if (!isRequired && OptionAsUnderfied(context))
                {
                    context.Builder.Append("?");
                }
                context.Builder.Append(": ");
                _typeLink.Generate(context);
            }

            if (!isRequired && OptionAsNullable(context))
            {
                context.Builder.Append(" | null");
            }
        }

        private bool PropertyIsRequired(IGeneratorModuleContext context)
        {
            if (_required != null && _required.Value)
            {
                return true;
            }

            if (_typeProperty != null)
            {
                return _typeProperty.GetCustomAttributes<RequiredAttribute>().Any() ||
                         !context.TypeInfo.IsNullable(_typeProperty.Type);
            }

            return false;
        }

        private bool OptionAsUnderfied(IGeneratorModuleContext context) => context.OptionalPropertiesStyle == OptionalPropertiesStyle.AsOptional;

        private bool OptionAsNullable(IGeneratorModuleContext context) => context.OptionalPropertiesStyle == OptionalPropertiesStyle.AsNullable;
    }
}