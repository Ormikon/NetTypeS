using System;
using System.Collections.Generic;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Dynamic
{
	internal class DynamicElement : IDynamicElement
	{
		private readonly Func<IEnumerable<ITypeScriptElement>> customFunction;

		public DynamicElement(Func<IEnumerable<ITypeScriptElement>> customFunction)
		{
			this.customFunction = customFunction;
		}

		public IEnumerable<ITypeScriptElement> GetElements()
		{
			return customFunction();
		}
	}
}