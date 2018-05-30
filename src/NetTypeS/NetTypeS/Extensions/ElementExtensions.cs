using System;
using System.Collections.Generic;
using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;

// ReSharper disable CheckNamespace

namespace NetTypeS
// ReSharper restore CheckNamespace
{
    public static class ElementExtensions
    {
        public static Element AddElement(this Element element, ITypeScriptElement e)
        {
            element.Add(e);
            return element;
        }

        public static Element AddBlock(this Element element, IEnumerable<ITypeScriptElement> elements, bool inline = false)
        {
            element.Add(new BlockElement(elements, inline));
            return element;
        }

        public static Element AddBlock(this Element element, params ITypeScriptElement[] elements)
        {
            element.Add(new BlockElement(elements));
            return element;
        }

        public static Element AddBlock(this Element element, ITypeScriptElement e)
        {
            element.Add(new BlockElement(new[] { e }));
            return element;
        }

        public static Element AddIndented(this Element element, IEnumerable<ITypeScriptElement> elements)
        {
            element.Add(new IndentedElement(elements));
            return element;
        }

        public static Element AddIndented(this Element element, params ITypeScriptElement[] elements)
        {
            element.Add(new IndentedElement(elements));
            return element;
        }

        public static Element AddIndented(this Element element, ITypeScriptElement e)
        {
            element.Add(new IndentedElement(new[] { e }));
            return element;
        }

        public static Element AddLine(this Element element)
        {
            element.Add(NewLineElement.Instance);
            return element;
        }

        public static Element AddText(this Element element, string text)
        {
            element.Add(new TextElement(text));
            return element;
        }

        public static Element AddTypeName(this Element element, Type type)
        {
            element.Add(new TypeNameElement(type));
            return element;
        }

        public static Element AddTypeName(this Element element, string name)
        {
            element.Add(new TypeNameElement(name));
            return element;
        }

        public static Element AddTypeLink(this Element element, Type type)
        {
            element.Add(new TypeLinkElement(type));
            return element;
        }

        public static Element AddTypeLink(this Element element, string moduleName, string typeName)
        {
            element.Add(new TypeLinkElement(moduleName, typeName));
            return element;
        }

        public static Element AddMultiLineText(this Element element, string text)
        {
            element.Add(new MultilineTextElement(text));
            return element;
        }

        public static Element AddEnumName(this Element element, string name)
        {
            element.Add(new EnumNameElement(name));
            return element;
        }

        public static Element AddEnumValue(this Element element, IEnumValue value)
        {
            element.Add(new EnumValueElement(value));
            return element;
        }

        public static Element AddEnumValue(this Element element, string valueName, int? value)
        {
            element.Add(new EnumValueElement(valueName, value));
            return element;
        }

        public static Element AddInterfaceName(this Element element, string name)
        {
            element.Add(new InterfaceNameElement(name));
            return element;
        }

        public static Element AddPropertyName(this Element element, string name)
        {
            element.Add(new PropertyNameElement(name));
            return element;
        }

        public static Element AddProperty(this Element element, ITypeProperty property)
        {
            element.Add(new PropertyElement(property));
            return element;
        }

        public static Element AddProperty(this Element element, ITypeProperty property, bool required)
        {
            element.Add(new PropertyElement(property, required));
            return element;
        }

        public static Element AddProperty(this Element element, string propertyName, Type propertyType)
        {
            element.Add(new PropertyElement(propertyName, propertyType));
            return element;
        }

        public static Element AddProperty(this Element element, string propertyName, Type propertyType, bool required)
        {
            element.Add(new PropertyElement(propertyName, propertyType, required));
            return element;
        }

        public static Element AddModuleName(this Element element, string name)
        {
            element.Add(new ModuleNameElement(name));
            return element;
        }

        public static Element AddIf(this Element element, Func<bool> test, Action<Element> action,
            Action<Element> elseAction = null)
        {
            if (test != null && action != null && test())
                action(element);
            else if (elseAction != null)
                elseAction(element);
            return element;
        }

        public static Element AddAll<T>(this Element element, IEnumerable<T> values, Action<Element, T> action)
        {
            if (values == null || action == null)
                return element;
            foreach (var value in values)
            {
                action(element, value);
            }
            return element;
        }

        public static Element AddAll<T>(this Element element, IEnumerable<T> values, Action<Element, T, int> action)
        {
            if (values == null || action == null)
                return element;
            int idx = 0;
            foreach (var value in values)
            {
                action(element, value, idx);
                idx++;
            }
            return element;
        }
    }
}