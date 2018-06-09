using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript code element with the fixed collection of inner elements.
    /// </summary>
    public class FixedElement : ITypeScriptElement
    {
        private readonly ICollection<ITypeScriptElement> _elements;

        /// <summary>
        /// Creates a new fixed element collection
        /// </summary>
        /// <param name="elements">Elements</param>
        public FixedElement(IEnumerable<ITypeScriptElement> elements)
            : this(elements?.ToArray())
        {
        }

        /// <summary>
        /// Creates a new fixed element collection
        /// </summary>
        /// <param name="elements">Elements</param>
        public FixedElement(params ITypeScriptElement[] elements)
        {
            _elements = elements ?? new ITypeScriptElement[0];
        }

        public IEnumerator<ITypeScriptElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ITypeScriptElement item)
        {
            throw new NotSupportedException("This operation is not supported for the fixed element.");
        }

        public void Clear()
        {
            throw new NotSupportedException("This operation is not supported for the fixed element.");
        }

        public bool Contains(ITypeScriptElement item)
        {
            return _elements.Contains(item);
        }

        public void CopyTo(ITypeScriptElement[] array, int arrayIndex)
        {
            _elements.CopyTo(array, arrayIndex);
        }

        public bool Remove(ITypeScriptElement item)
        {
            throw new NotSupportedException("This operation is not supported for the fixed element.");
        }

        public int Count => _elements.Count;

        public bool IsReadOnly => true;

        public virtual void Generate(IGeneratorModuleContext context)
        {
            foreach (var element in _elements)
            {
                element.Generate(context);
            }
        }
    }
}
