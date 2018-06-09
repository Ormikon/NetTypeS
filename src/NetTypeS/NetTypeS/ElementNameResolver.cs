using System;
using NetTypeS.Interfaces;
using NetTypeS.Types;
using NetTypeS.Utils;

namespace NetTypeS
{
    internal class ElementNameResolver : IElementNameResolver
    {
        private readonly ITypeCollector _collector;
        private readonly ICustomTypeNameHolder _customTypeNameHolder;
        private readonly IGeneratorSettings _settings;

        public ElementNameResolver(ITypeCollector collector,
            ICustomTypeNameHolder customTypeNameHolder, IGeneratorSettings settings)
        {
            _collector = collector;
            _customTypeNameHolder = customTypeNameHolder;
            _settings = settings;
        }

        public string GetTypeName(Type type)
        {
            if (type == null)
                return SimpleType.Any.Name;
            var tst = _collector.Get(type);
            if (tst != null && tst.Code == TypeScriptTypeCode.Nullable)
                type = ((INullableType)tst).UnderlyingType;
            // for not collected nullable
            else if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }
            var name = _customTypeNameHolder.GetNameFor(type);
            if (!string.IsNullOrEmpty(name))
                return name;
            tst = _collector.Get(type);
            if (tst == null)
                return SimpleType.Any.Name;
            if (tst.Code == TypeScriptTypeCode.Enum || tst.Code == TypeScriptTypeCode.Complex)
                return _settings.TypeNameResolver(tst);
            return tst.Name;
        }

        public string GetPropertyName(ITypeProperty property)
        {
            return _settings.PropertyNameResolver(property);
        }

        public string GetEnumValueName(IEnumValue enumValue)
        {
            return enumValue.Name;
        }
    }
}