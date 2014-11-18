using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetTypeS.Interfaces;
using NetTypeS.Types;

namespace NetTypeS.Utils
{
	public static class TypeUtils
	{
		public static readonly IDictionary<Type, Type> KnownTypes = new Dictionary<Type, Type>
		                                                            {
			                                                            {typeof (DateTime), typeof (string)},
			                                                            {typeof (DateTimeOffset), typeof (string)},
			                                                            {typeof (Guid), typeof (string)},
			                                                            {typeof (TimeSpan), typeof (string)},
			                                                            {typeof (Uri), typeof (string)},
			                                                            {typeof (IntPtr), typeof (long)},
			                                                            {typeof (UIntPtr), typeof (ulong)}
		                                                            };

		public static void ThrowIfUnsupported(this Type type)
		{
			/*if (type == typeof(IntPtr) || type == typeof(UIntPtr))
                throw new ArgumentException("Ptr types are unsupported.", "type");*/
		}

		public static bool IsNullable(this Type type)
		{
			if (type.IsValueType && type.IsGenericType && type.IsConstructedGenericType)
			{
				var genericType = type.GetGenericTypeDefinition();
				if (genericType == typeof (Nullable<>))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsCollection(this Type type)
		{
			return type.GetCollectionType() != null;
		}

		public static bool IsEnum(this Type type)
		{
			return type.IsValueType && type.IsEnum;
		}

		public static bool IsSimple(this Type type)
		{
			if (typeof (object) == type || typeof (string) == type || typeof (void) == type)
				return true;
			if (type.IsValueType)
			{
				if (type.IsEnum)
					return false;
				if (type.IsPrimitive)
					return true;
				var tc = Type.GetTypeCode(type);
				switch (tc)
				{
					case TypeCode.DBNull:
					case TypeCode.DateTime:
					case TypeCode.Decimal:
					case TypeCode.Empty:
						return true;
				}
			}
			return false;
		}

		public static Type GetCollectionType(this Type type)
		{
			var enumType = type == typeof (IEnumerable<>)
				? type
				: type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>)
					? type
					: type.GetInterfaces().FirstOrDefault(i => i == typeof(IEnumerable<>));
			if (enumType != null && enumType.GenericTypeArguments != null
			    && enumType.GenericTypeArguments.Length > 0)
			{
				return enumType.GenericTypeArguments.First();
			}
			enumType = type == typeof (IEnumerable) ? type : type.GetInterfaces().FirstOrDefault(i => i == typeof (IEnumerable));
			return enumType == null ? null : typeof (object);
		}

		public static IEnumValue[] GetEnumTypeValues(this Type type)
		{
			if (!type.IsEnum)
				throw new ArgumentException("Only enum types supported.", "type");
			return type.GetEnumNames()
				.Zip(Enum.GetValues(type)
					.Cast<object>(),
					(m, v) =>
					{
						var mi = type.GetMember(m).First();
						return new EnumTypeValue(m, v, mi);
					})
				.Cast<IEnumValue>()
				.ToArray();
		}

		public static MemberInfo[] GetPublicMembers(this Type type)
		{
			return type.GetMembers().Where(m =>
			                               {
				                               if (m.MemberType == MemberTypes.Field)
				                               {
					                               var fi = (FieldInfo) m;
					                               return fi.IsPublic && !fi.IsStatic;
				                               }

				                               if (m.MemberType == MemberTypes.Property)
				                               {
					                               var pi = (PropertyInfo) m;
					                               return pi.CanRead && pi.GetMethod != null && pi.GetMethod.IsPublic;
				                               }
				                               return false;
			                               }).ToArray();
		}
	}
}