using System;
using NetTypeS.Interfaces;

namespace NetTypeS
{
    internal class TypeInfo : ITypeInfo
    {
        private readonly ITypeCollector _collector;

        public TypeInfo(ITypeCollector collector)
        {
            _collector = collector;
        }

        public bool IsNullable(Type type)
        {
            var tst = _collector.Get(type);
            return tst == null || !tst.IsRequired;
        }
    }
}