using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;
using NetTypeS.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace NetTypeS.WebApi
{
    class EndpointInfo
    {
        public string ActionName { get; }
        public string GeneratedName { get; set; }
        public string ControllerName { get; }
        public string HttpMethodName { get; }
        public string RelativePath { get; }
        public ParameterInfo[] Parameters { get; }
        public Type ResponseType { get; }

        public EndpointInfo(ApiDescription api)
        {
            ActionName = api.ActionDescriptor.ActionName;
            GeneratedName = NetTypeS.Utils.StringUtils.ToCamelCase(ActionName);
            ControllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
            HttpMethodName = api.HttpMethod.Method;
            RelativePath = api.RelativePath;

            ResponseType = Utils.ReplaceUnsupportedTypesWithAny(
                api.ResponseDescription.ResponseType ?? api.ResponseDescription.DeclaredType
            );

            Parameters = api.ParameterDescriptions.Select(param => new ParameterInfo(param)).ToArray();
        }

        public void RegisterTypes(IGeneratorModule module)
        {
            module.Include(ResponseType);
            Parameters.ForEach(p => p.RegisterTypes(module));
        }

        public ITypeScriptElement GenerateFunction(string promiseType)
        {
            var method = Element.New()
                .AddText(GeneratedName)
                .AddText(": function(");

            if (Parameters.Count(p => !p.IsQuery) > 1)
            {
                throw new ApplicationException(string.Format(
                    "More than one request body parameter for method {0}.{1}. Consider making request class, or using query parameters",
                    ControllerName, ActionName
                ));
            }

            Parameters.ForEach((p, n) =>
            {
                method.AddText(p.GeneratedName);
                method.AddText(" : ");
                method.AddTypeLink(p.GeneratedType);

                if (n != Parameters.Length - 1)
                {
                    method.AddText(", ");
                }
            });

            method.AddText(")");

            method.AddText(" : " + promiseType + "<");
            if (ResponseType != null)
            {
                method.AddTypeLink(ResponseType);
            }
            else
            {
                method.AddText("void");
            }
            method.AddText("> ");

            var apiCallBlock = Element.New()
                    .AddText($"return processRequest(")
                    .AddText("`/" + ReplaceQueryPlaceholders(RelativePath) + "`");

            apiCallBlock
                .AddText(", ")
                .AddText($"`{HttpMethodName}`");

            if (Parameters.Any(p => !p.IsQuery))
            {
                apiCallBlock
                    .AddText(", ")
                    .AddText(Parameters.FirstOrDefault(p => !p.IsQuery).GeneratedName);
            }

            apiCallBlock.AddText(")");

            method.AddBlock(apiCallBlock);

            return method;
        }

        private static string ReplaceQueryPlaceholders(string url)
        {
            return url.Replace("{", "${");
        }


    }
}
