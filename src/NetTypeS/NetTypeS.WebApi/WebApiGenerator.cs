using NetTypeS.Utils;
using NetTypeS.Elements.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using NetTypeS.WebApi.ApiModels;

namespace NetTypeS.WebApi
{
    internal class WebApiGenerator
    {
        private readonly string _promiseType;
        private readonly string _apiModuleName;

        public WebApiGenerator(string promiseType = "Promise", string apiModuleName = "apiDefinition"
        )
        {
            _promiseType = promiseType;
            _apiModuleName = apiModuleName;
        }

        public GeneratedFiles GenerateAll(IEnumerable<EndpointInfo> controllers, IEnumerable<Type> additionalTypes)
        {
            var types = Generator.New(new GeneratorSettings { IncludeInheritedTypes = true, GenerateNumberTypeForDictionaryKeys = true });
            
            types.Module("models", m =>
            {
                m.Include(controllers.Select(x => x.ResponseType));
                m.Include(controllers.SelectMany(x => x.Parameters).Select(x => x.GeneratedType));
                additionalTypes?.ForEach((t, b) => m.Include(t));

                EnumHelper.GenerateEnumNameLookups(m);
            });

            var endpointsByController = controllers.ToLookup(c => c.ControllerName);

            types.Module(_apiModuleName, apiBuilder =>
            {
                apiBuilder.Import("models", "./models");

                var wrapperFunction = Element.New();
                wrapperFunction.AddText("export function getApi(processRequest: (path: string, method: string, data?: any) => Promise<any>) {");
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
                        .AddText(StringUtils.ToCamelCase(controllerEndpointsGroup.Key))
                        .AddText(": ");

                    var methodsBlockContent = Element.New();

                    var controllerEndpoints = controllerEndpointsGroup
                        .GroupBy(e => e.ActionName)
                        .SelectMany(RenameOverloads);

                    controllerEndpoints
                        .Select(m => FunctionHelper.GenerateFunction(m, _promiseType))
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
                Api = types.GenerateModule(this._apiModuleName)
            };
        }

        private IEnumerable<EndpointInfo> RenameOverloads(IEnumerable<EndpointInfo> endpoints)
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
