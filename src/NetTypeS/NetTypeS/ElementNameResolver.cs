using System;
using NetTypeS.Interfaces;
using NetTypeS.Types;
using NetTypeS.Utils;

namespace NetTypeS
{
	internal class ElementNameResolver : IElementNameResolver
	{
		private readonly ITypeCollector collector;
		private readonly ICustomTypeNameHolder customTypeNameHolder;
		private readonly IGeneratorSettings settings;

		public ElementNameResolver(ITypeCollector collector,
			ICustomTypeNameHolder customTypeNameHolder, IGeneratorSettings settings)
		{
			this.collector = collector;
			this.customTypeNameHolder = customTypeNameHolder;
			this.settings = settings;
		}

		public string GetTypeName(Type type)
		{
			if (type == null)
				return SimpleType.Any.Name;
			var tst = collector.Get(type);
			if (tst != null && tst.Code == TypeScriptTypeCode.Nullable)
				type = ((INullableType) tst).UnderlyingType;
				// for not collected nullable
			else if (type.IsNullable())
			{
				type = Nullable.GetUnderlyingType(type);
			}
			var name = customTypeNameHolder.GetNameFor(type);
			if (!string.IsNullOrEmpty(name))
				return name;
			tst = collector.Get(type);
			if (tst == null)
				return SimpleType.Any.Name;
			if (tst.Code == TypeScriptTypeCode.Enum || tst.Code == TypeScriptTypeCode.Complex)
				return settings.TypeNameResolver(tst);
			return tst.Name;
		}

		public string GetPropertyName(ITypeProperty property)
		{
			return settings.PropertyNameResolver(property);
		}

		public string GetEnumValueName(IEnumValue enumValue)
		{
			return enumValue.Name;
		}
	}
}