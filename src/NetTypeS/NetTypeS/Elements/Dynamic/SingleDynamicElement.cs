using System;
using System.Collections.Generic;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Dynamic
{
    internal class SingleDynamicElement : IDynamicElement
    {
        private readonly ITypeScriptElement _element;
        private readonly Func<ITypeScriptElement> _customFunction;

        public SingleDynamicElement(Func<ITypeScriptElement> customFunction)
        {
            _customFunction = customFunction;
            _element = null;
        }

        public SingleDynamicElement(ITypeScriptElement element)
        {
            _element = element;
            _customFunction = null;
        }

        public IEnumerable<ITypeScriptElement> GetElements()
        {
            yield return _customFunction == null ? _element : _customFunction();
        }
    }
}
