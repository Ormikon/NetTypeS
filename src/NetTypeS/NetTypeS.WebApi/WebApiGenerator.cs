using NetTypeS;
using NetTypeS.Utils;
using NetTypeS.WebApi.Utils;
using NetTypeS.Elements.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;
using NetTypeS.Interfaces;
using System.Diagnostics;

namespace NetTypeS.WebApi
{
    public class GeneratedFiles
    {
        public string Models { get; set; }
        public string Api { get; set; }
    }

    public class WebApiGenerator
    {
        string promiseType;
        string apiModuleName;
        Func<ApiDescription, bool> apiFilter;

        public WebApiGenerator(
            string promiseType = "Promise",
            string apiModuleName = "apiDefinition",
            Func<ApiDescription, bool> apiFilter = null
        )
        {
            this.promiseType = promiseType;
            this.apiModuleName = apiModuleName;
            this.apiFilter = apiFilter;
        }

        public GeneratedFiles GenerateAll(
            IApiExplorer explorer,
            IEnumerable<Type> additionalTypes = null)
        {
            var types = Generator.New(new GeneratorSettings { IncludeInheritedTypes = true, GenerateNumberTypeForDictionaryKeys = true });

            var apiDescriptions = explorer.ApiDescriptions.AsEnumerable();

            if (this.apiFilter != null)
            {
                apiDescriptions = apiDescriptions.Where(a => this.apiFilter(a));
            };

            var modelsModule = types.Module("models", m =>
            {
                RegisterTypes(m, apiDescriptions);

                if (additionalTypes != null)
                {
                    additionalTypes.ForEach((t, b) => m.Include(t));
                }

                m.ForEnums(et =>
                    Element.New()
                        .AddText("export var ")
                        .AddText(NetTypeS.Utils.StringUtils.ToCamelCase(et.Name))
                        .AddText("Names = ")
                        .AddBlock(et.Values.Select((ev, i) =>
                            Element.New()
                                .AddText(ev.ValueAsInt32().ToString(CultureInfo.InvariantCulture))
                                .AddText(": \"")
                                .AddText(
                                    ev.CustomAttributes.OfType<DisplayAttribute>().Select(a => a.Name).SingleOrDefault()
                                    ?? PascalCaseToWords(ev.Name)
                                )
                                .AddText("\"")
                                .AddIf(() => i != et.Values.Count - 1, e => e.AddText(","))))
                        .AddText(";")
                        .AddLine()
                );
            });

            var endpointsByController = apiDescriptions.ToLookup(a => a.ActionDescriptor.ControllerDescriptor.ControllerName);

            var apiModule = types.Module(this.apiModuleName, apiBuilder =>
            {
                apiBuilder.Import("models", "./models");

                var wrapperFunction = Element.New();
                wrapperFunction.AddText("export function getApi(processRequest) {");
                wrapperFunction.AddLine();

                var apiDefinitionObject = Element.New();

                endpointsByController.ForEach((controllerEndpointsGroup, controllerN) =>
                {
                    if (controllerN > 0)
                    {
                        apiDefinitionObject.AddText(",");
                        apiDefinitionObject.AddLine();
                    }

                    var contollerElement = Element.New()
                        .AddText(NetTypeS.Utils.StringUtils.ToCamelCase(controllerEndpointsGroup.Key))
                        .AddText(": ");

                    var methodsBlockContent = Element.New();

                    controllerEndpointsGroup
                        .Select(m => GenerateFunction(m))
                        .ForEach((m, n) =>
                        {
                            if (n > 0)
                            {
                                methodsBlockContent.AddText(",");
                                methodsBlockContent.AddLine();
                            }
                            methodsBlockContent.Add(m);
                        });

                    contollerElement.AddBlock(methodsBlockContent);

                    apiDefinitionObject.Add(contollerElement);
                });

                var wrapperFunctionBody = Element.New()
                    .AddText("return {")
                    .AddLine()
                    .AddIndented(apiDefinitionObject)
                    .AddText("};");

                wrapperFunction.AddIndented(wrapperFunctionBody);
                wrapperFunction.AddText("}");

                apiBuilder.Element(wrapperFunction);
            });

            return new GeneratedFiles
            {
                Models = types.GenerateModule("models"),
                Api = types.GenerateModule(this.apiModuleName)
            };
        }

        private ITypeScriptElement GenerateFunction(ApiDescription endpoint)
        {
            var actionName = endpoint.ActionDescriptor.ActionName;

            var method = Element.New()
                .AddText(NetTypeS.Utils.StringUtils.ToCamelCase(actionName))
                .AddText(": function(");

            var parameters = endpoint.ParameterDescriptions.Select(p => new
            {
                Name = NetTypeS.Utils.StringUtils.ToCamelCase(p.Name),
                DataType = p.ParameterDescriptor.ParameterType,
                IsQuery = p.Source == ApiParameterSource.FromUri
            }).ToArray();

            if (parameters.Count(p => !p.IsQuery) > 1)
            {
                throw new ApplicationException(string.Format(
                    "More than one request body parameter for method {0}.{1}. Consider making request class, or using query parameters",
                    endpoint.ActionDescriptor.ControllerDescriptor.ControllerName,
                    actionName));
            }

            parameters.ForEach((p, n) =>
            {
                method.AddText(p.Name);
                method.AddText(" : ");
                method.AddTypeLink(p.DataType);

                if (n != parameters.Length - 1)
                {
                    method.AddText(", ");
                }
            });

            method.AddText(")");

            method.AddText(" : " + promiseType + "<");
            if (endpoint.ActionDescriptor.ReturnType != null)
            {
                method.AddTypeLink(endpoint.ActionDescriptor.ReturnType);
            }
            else
            {
                method.AddText("void");
            }
            method.AddText("> ");

            var apiCallBlock = Element.New()
                    .AddText($"return processRequest(")
                    .AddText("`/" + ReplaceQueryPlaceholders(endpoint.RelativePath) + "`");

            apiCallBlock
                .AddText(", ")
                .AddText($"`{endpoint.HttpMethod.Method}`");

            if (parameters.Any(p => !p.IsQuery))
            {
                apiCallBlock
                    .AddText(", ")
                    .AddText(parameters.FirstOrDefault(p => !p.IsQuery).Name);
            }

            apiCallBlock.AddText(")");

            method.AddBlock(apiCallBlock);

            return method;
        }

        private void RegisterTypes(IGeneratorModule module, IEnumerable<ApiDescription> descriptions)
        {
            foreach (var description in descriptions)
            {
                module.Include(description.ResponseDescription.ResponseType ??
                               description.ResponseDescription.DeclaredType);
                foreach (var parameterDescription in description.ParameterDescriptions)
                {
                    module.Include(parameterDescription.ParameterDescriptor.ParameterType);
                }
            }
        }

        private static string ReplaceQueryPlaceholders(string url)
        {
            return url.Replace("{", "${");
        }

        private static string PascalCaseToWords(string ident)
        {
            if (ident == null)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            for (var n = 0; n < ident.Length; n++)
            {
                if (n > 0 && Char.IsUpper(ident[n]) && n < ident.Length - 1 && Char.IsLower(ident[n + 1]))
                {
                    sb.Append(' ');
                }
                sb.Append(ident[n]);
            }
            return sb.ToString();
        }
    }
}
