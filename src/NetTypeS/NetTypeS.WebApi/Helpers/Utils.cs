using System;
using System.Collections.Generic;
using System.Net.Http;

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

        public static Type ReplaceUnsupportedTypesWithAny(Type type)
        {
            if (type == typeof(HttpResponseMessage))
            {
                return typeof(object);
            }

            if (type == typeof(HttpRequestMessage))
            {
                return typeof(object);
            }

            return type;
        }
    }
}
