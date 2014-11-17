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
		private readonly ICollection<ITypeScriptElement> elements;

		/// <summary>
		/// Creates a new fixed element collection
		/// </summary>
		/// <param name="elements">Elements</param>
		public FixedElement(IEnumerable<ITypeScriptElement> elements)
			: this(elements == null ? null : elements.ToArray())
		{
		}

		/// <summary>
		/// Creates a new fixed element collection
		/// </summary>
		/// <param name="elements">Elements</param>
		public FixedElement(params ITypeScriptElement[] elements)
		{
			this.elements = elements ?? new ITypeScriptElement[0];
		}

		public IEnumerator<ITypeScriptElement> GetEnumerator()
		{
			return elements.GetEnumerator();
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
			return elements.Contains(item);
		}

		public void CopyTo(ITypeScriptElement[] array, int arrayIndex)
		{
			elements.CopyTo(array, arrayIndex);
		}

		public bool Remove(ITypeScriptElement item)
		{
			throw new NotSupportedException("This operation is not supported for the fixed element.");
		}

		public int Count
		{
			get { return elements.Count; }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		public virtual void Generate(IGeneratorModuleContext context)
		{
			foreach (var element in elements)
			{
				element.Generate(context);
			}
		}
	}
}