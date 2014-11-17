using System;
using NetTypeS.Interfaces;

namespace NetTypeS
{
	internal class TypeInfo : ITypeInfo
	{
		private readonly ITypeCollector collector;

		public TypeInfo(ITypeCollector collector)
		{
			this.collector = collector;
		}

		public bool IsNullable(Type type)
		{
			var tst = collector.Get(type);
			return tst == null || !tst.IsRequired;
		}
	}
}