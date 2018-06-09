using System;
using NetTypeS.Interfaces;

// ReSharper disable CheckNamespace

namespace NetTypeS
// ReSharper restore CheckNamespace
{
	public static class TypeCollectorExtensions
	{
		public static TSType Get<TSType>(this ITypeCollector collector, Type type)
			where TSType : ITypeScriptType
		{
			var tst = collector.Get(type);
			return tst is TSType sType ? sType : default(TSType);
		}
	}
}