using System;
using System.Linq;
using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;
using NetTypeS.WebApi.Models;

namespace NetTypeS.WebApi.Helpers
{
    internal static class FunctionHelper
    {
        public static ITypeScriptElement GenerateFunction(EndpointInfo controller, string promiseType)
        {
            var method = Element.New()
                .AddText(controller.GeneratedName)
                .AddText(": function(");

            if (controller.Parameters.Count(p => !p.IsQuery) > 1)
            {
                throw new ApplicationException(string.Format(
                    "More than one request body parameter for method {0}.{1}. Consider making request class, or using query parameters",
                    controller.ControllerName, controller.ActionName
                ));
            }

            controller.Parameters.ForEach((p, n) =>
            {
                method.AddText(p.GeneratedName);
                method.AddText(" : ");
                method.AddTypeLink(p.GeneratedType);

                if (n != controller.Parameters.Length - 1)
                    method.AddText(", ");
            });

            method.AddText(")");

            method.AddText(" : " + promiseType + "<");
            if (controller.ResponseType != null)
                method.AddTypeLink(controller.ResponseType);
            else
                method.AddText("void");

            method.AddText("> ");

            var apiCallBlock = Element.New()
                    .AddText("return processRequest(")
                    .AddText("`/" + ReplaceQueryPlaceholders(controller.RelativePath) + "`");

            apiCallBlock
                .AddText(", ")
                .AddText($"`{controller.HttpMethodName}`");

            if (controller.Parameters.Any(p => !p.IsQuery))
            {
                apiCallBlock
                    .AddText(", ")
                    .AddText(controller.Parameters.FirstOrDefault(p => !p.IsQuery).GeneratedName);
            }

            apiCallBlock.AddText(");");

            method.AddBlock(apiCallBlock);

            return method;
        }

        private static string ReplaceQueryPlaceholders(string url)
        {
            return url.Replace("{", "${");
        }
    }
}
