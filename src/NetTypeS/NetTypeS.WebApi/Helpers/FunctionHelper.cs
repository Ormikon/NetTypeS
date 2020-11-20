using System;
using System.Linq;
using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;
using NetTypeS.Utils;
using NetTypeS.WebApi.Models;

namespace NetTypeS.WebApi.Helpers
{
    internal static class FunctionHelper
    {
        public static ITypeScriptElement GenerateFunction(EndpointInfo controller, string promiseType, bool queryParametersAsObject)
        {
            var method = Element.New()
                .AddText(controller.GeneratedName)
                .AddText(": function(");

            if (controller.Parameters.Count(p => p.IsBody) > 1)
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

            var bodyParam = controller.Parameters.FirstOrDefault(p => p.IsBody);

            if (bodyParam != null)
            {
                apiCallBlock
                    .AddText(", ")
                    .AddText(bodyParam.GeneratedName);
            }

            if (queryParametersAsObject)
            {
                AppendQueryParametersAsObject(controller, apiCallBlock);
            }

            apiCallBlock.AddText(");");

            method.AddBlock(apiCallBlock);

            return method;
        }

        private static string ReplaceQueryPlaceholders(string url)
        {
            return url.Replace("{", "${");
        }

        private static void AppendQueryParametersAsObject(EndpointInfo controller, Element apiCallBlock)
        {
            var queryToClass = controller.Parameters.Where(p => p.IsQuery && !p.GeneratedType.IsSimple() && !p.GeneratedType.IsEnum());

            if (queryToClass.Count() > 1)
            {
                throw new ApplicationException(string.Format(
                    "More than one request query parameter for method {0}.{1} is mapped to class. Consider making one class, or using query parameters",
                    controller.ControllerName, controller.ActionName
                ));
            }

            if (queryToClass.Any())
            {
                apiCallBlock
                    .AddText(", ")
                    .AddText(queryToClass.Single().GeneratedName);
            }
            else
            {
                var simpleQueyParameters = controller.Parameters.Where(p => p.IsQuery && (p.GeneratedType.IsSimple() || p.GeneratedType.IsEnum()));

                if (simpleQueyParameters.Any())
                {
                    apiCallBlock
                        .AddText(", {");

                    var tsProperty = Element.New();

                    foreach (var p in simpleQueyParameters)
                    {
                        tsProperty.AddLine()
                            .AddText(p.GeneratedName)
                            .AddText(": ")
                            .AddText(p.GeneratedName)
                            .AddText(",");
                    }

                    apiCallBlock
                        .AddIndented(tsProperty)
                        .AddText("}");
                }

            }
        }
    }
}
