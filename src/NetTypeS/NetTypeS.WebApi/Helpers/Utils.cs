using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetTypeS.WebApi.Helpers
{
    public static class Utils
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            if (collection == null)
            {
                return;
            }

            var i = 0;
            foreach (var entity in collection)
            {
                action(entity, i);
                i++;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection.ForEach((i, n) => action(i));
        }

        public static Type ReplaceUnsupportedTypes(Type type)
        {
            type = GetTypeIfTask(type);

            if (type == typeof(HttpResponseMessage))
                return typeof(object);

            if (type == typeof(HttpRequestMessage))
                return typeof(object);

            return type;
        }

        private static Type GetTypeIfTask(Type type)
        {
            if (type == typeof(Task) || type == null)
                return typeof(void);

            if (type.IsGenericType && type.IsConstructedGenericType)
            {
                var genericTypeDefenition = type.GetGenericTypeDefinition();
                if (genericTypeDefenition == typeof(Task<>))
                    return type.GenericTypeArguments[0];
            }

            return type;
        }
    }
}
