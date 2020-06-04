using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetTypeS.WebApi.Core.Helpers
{
    public static class UtilsCore
    {
        public static Type ReplaceUnsupportedTypes(Type type)
        {
            type = WebApi.Helpers.Utils.ReplaceUnsupportedTypes(type);

            type = GetTypeIfActionResult(type);

            return type;
        }

        private static Type GetTypeIfActionResult(Type type)
        {
            if (type == null)
            {
                return typeof(void);
            }

            if (typeof(IActionResult).IsAssignableFrom(type))
            {
                return typeof(void);
            }

            if (type.IsGenericType && type.IsConstructedGenericType)
            {
                var genericTypeDefenition = type.GetGenericTypeDefinition();
                if (genericTypeDefenition == typeof(ActionResult<>))
                {
                    return type.GenericTypeArguments[0];
                }
            }

            return type;
        }
    }
}
