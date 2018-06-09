using System.Collections;
using System.Collections.Generic;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
    /// <summary>
    /// TypeScript code element to build elements collection
    /// </summary>
    public class Element : ITypeScriptElement
    {
        private readonly ICollection<ITypeScriptElement> _collection = new List<ITypeScriptElement>();

        /// <summary>
        /// Creates a new TypeScript code elements collection
        /// </summary>
        public Element() : this(null)
        {
        }

        /// <summary>
        /// Creates a new TypeScript code elements collection
        /// </summary>
        /// <param name="elements">Collection elements</param>
        public Element(IEnumerable<ITypeScriptElement> elements)
        {
            if (elements != null)
                ((List<ITypeScriptElement>)_collection).AddRange(elements);
        }

        /// <summary>
        /// Creates a new TypeScript code elements collection
        /// </summary>
        /// <param name="elements">Collection elements</param>
        public Element(params ITypeScriptElement[] elements)
            : this((IEnumerable<ITypeScriptElement>)elements)
        {
        }

        /// <summary>
        /// Creates a new TypeScript code elements collection
        /// </summary>
        public static Element New()
        {
            return new Element();
        }

        public IEnumerator<ITypeScriptElement> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ITypeScriptElement item)
        {
            _collection.Add(item);
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool Contains(ITypeScriptElement item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(ITypeScriptElement[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        public bool Remove(ITypeScriptElement item)
        {
            return _collection.Remove(item);
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => false;

        public void Generate(IGeneratorModuleContext context)
        {
            foreach (var e in _collection)
            {
                e.Generate(context);
            }
        }
    }
}
