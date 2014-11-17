using System;
using System.Collections;
using System.Collections.Generic;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Primitives
{
	/// <summary>
	/// TypeScript code element without elements inside. Useful for creating basic elements.
	/// </summary>
	public abstract class EmptyElement : ITypeScriptElement
	{
		public static readonly IEnumerable<ITypeScriptElement> EmptyEnumerable = new ITypeScriptElement[0];

		public IEnumerator<ITypeScriptElement> GetEnumerator()
		{
			return EmptyEnumerable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(ITypeScriptElement item)
		{
			throw new NotSupportedException("Is not supported for empty TypeScript element.");
		}

		public void Clear()
		{
			throw new NotSupportedException("Is not supported for empty TypeScript element.");
		}

		public bool Contains(ITypeScriptElement item)
		{
			throw new NotSupportedException("Is not supported for empty TypeScript element.");
		}

		public void CopyTo(ITypeScriptElement[] array, int arrayIndex)
		{
			throw new NotSupportedException("Is not supported for empty TypeScript element.");
		}

		public bool Remove(ITypeScriptElement item)
		{
			throw new NotSupportedException("Is not supported for empty TypeScript element.");
		}

		public int Count
		{
			get { return 0; }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		public abstract void Generate(IGeneratorModuleContext context);
	}
}