using System;
using System.Collections.Generic;
using NetTypeS.Interfaces;

namespace NetTypeS.Elements.Dynamic
{
	internal class SingleDynamicElement : IDynamicElement
	{
		private readonly ITypeScriptElement element;
		private readonly Func<ITypeScriptElement> customFunction;

		public SingleDynamicElement(Func<ITypeScriptElement> customFunction)
		{
			this.customFunction = customFunction;
			element = null;
		}

		public SingleDynamicElement(ITypeScriptElement element)
		{
			this.element = element;
			customFunction = null;
		}

		public IEnumerable<ITypeScriptElement> GetElements()
		{
			yield return customFunction == null ? element : customFunction();
		}
	}
}