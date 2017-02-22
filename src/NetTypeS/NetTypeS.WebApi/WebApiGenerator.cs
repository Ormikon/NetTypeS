using NetTypeS;
using NetTypeS.Utils;
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
using System.Net.Http;

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

            var controllers = apiDescriptions.Select(api => new EndpointInfo(api));
            
            var modelsModule = types.Module("models", m =>
            {
                controllers.ForEach(c => c.RegisterTypes(m));

                if (additionalTypes != null)
                {
                    additionalTypes.ForEach((t, b) => m.Include(t));
                }

                EnumHelper.GenerateEnumNameLookups(m);
            });

            var endpointsByController = controllers.ToLookup(c => c.ControllerName);

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

                    var controllerEndpoints = controllerEndpointsGroup
                        .GroupBy(e => e.ActionName)
                        .SelectMany(overloads => RenameOverloads(overloads));

                    controllerEndpoints
                        .Select(m => m.GenerateFunction(promiseType))
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

        IEnumerable<EndpointInfo> RenameOverloads(IEnumerable<EndpointInfo> endpoints)
        {
            var needToAppendMethod = endpoints.GroupBy(e => e.HttpMethodName).Count() > 1;

            if (needToAppendMethod)
            {
                endpoints.ForEach(e => e.GeneratedName = StringUtils.ToCamelCase(e.HttpMethodName + e.ActionName));
                return endpoints.GroupBy(e => e.GeneratedName).SelectMany(g => RenameOverloads(endpoints));
            }

            endpoints.ForEach((e, n) => e.GeneratedName = e.GeneratedName + (n > 0 ? n.ToString() : ""));

            return endpoints;
        }
    }
}
