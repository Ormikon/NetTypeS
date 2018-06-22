using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
                if (genericType == typeof(Nullable<>))
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

        public static bool IsDictionary(this Type type)
        {
            return type.GetDictionaryTypes() != null;
        }

        public static bool IsEnum(this Type type)
        {
            return type.IsValueType && type.IsEnum;
        }

        public static bool IsNumber(this Type type)
        {
            if (type.IsValueType)
            {
                var tc = Type.GetTypeCode(type);
                switch (tc)
                {
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Decimal:
                        return true;
                }
            }
            return false;
        }

        public static bool IsSimple(this Type type)
        {
            if (typeof(object) == type || typeof(string) == type || typeof(void) == type)
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

        private static Type GetTypeInterface(Type definition, Type type)
        {
            if (!definition.IsInterface)
                throw new ArgumentException("Method supports only interface types as definitions");

            if (definition.IsGenericType)
            {
                if (definition == type)
                    return type;
                if (type.IsGenericType && type.IsConstructedGenericType)
                {
                    var typeDefinition = type.GetGenericTypeDefinition();
                    if (typeDefinition == definition)
                        return type;
                }
                var foundInterface =
                    type.GetInterfaces()
                        .FirstOrDefault(i => i == definition || (i.IsConstructedGenericType && i.GetGenericTypeDefinition() == definition));
                return foundInterface;
            }

            return type == definition ? type : type.GetInterfaces().FirstOrDefault(i => i == definition);
        }

        public static Type GetCollectionType(this Type type)
        {
            var interfaceType = GetTypeInterface(typeof(IEnumerable<>), type);
            if (interfaceType != null)
            {
                if (interfaceType == typeof(IEnumerable<>) || interfaceType.GenericTypeArguments.Length == 0)
                    return interfaceType.GetGenericArguments().First();
                return interfaceType.GenericTypeArguments.First();
            }

            interfaceType = GetTypeInterface(typeof(IEnumerable), type);
            return interfaceType == null ? null : typeof(object);
        }

        public static Type[] GetDictionaryTypes(this Type type)
        {
            var dictionaryInterface = GetTypeInterface(typeof(IDictionary<,>), type);
            if (dictionaryInterface != null)
            {
                //TODO: fix for generics
                return dictionaryInterface.GenericTypeArguments.ToArray();
            }
            return null;
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
                                                   var fi = (FieldInfo)m;
                                                   return fi.IsPublic && !fi.IsStatic;
                                               }

                                               if (m.MemberType == MemberTypes.Property)
                                               {
                                                   var pi = (PropertyInfo)m;
                                                   return pi.CanRead && pi.GetMethod != null && pi.GetMethod.IsPublic;
                                               }
                                               return false;
                                           }).ToArray();
        }
    }
}