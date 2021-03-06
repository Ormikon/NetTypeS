﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;
using NetTypeS.Utils;
using NetTypeS.WebApi.Helpers;
using NetTypeS.WebApi.Models;

namespace NetTypeS.WebApi
{
    public class WebApiGenerator
    {
        private readonly string _promiseType;
        private readonly string _apiModuleName;
        private readonly GeneratorSettings _settings;

        public WebApiGenerator(string promiseType = "Promise", string apiModuleName = "apiDefinition", Action<GeneratorSettings> setupSettings = null
        )
        {
            _promiseType = promiseType;
            _apiModuleName = apiModuleName;

            _settings = new GeneratorSettings
            {
                IncludeInheritedTypes = true,
                GenerateNumberTypeForDictionaryKeys = true,
            };

            setupSettings?.Invoke(_settings);
        }

        public IGeneratorSettings Settings => _settings;

        public GeneratedFiles GenerateAll(IEnumerable<EndpointInfo> controllers, IEnumerable<Type> additionalTypes)
        {
            var types = Generator.New(_settings);

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
                wrapperFunction.AddText($"export function getApi(processRequest: (path: string, method: string, data?: any) => {_promiseType}<any>) {{");
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
                        .Select(m => FunctionHelper.GenerateFunction(m, _promiseType, apiBuilder.Generator.Settings.QueryParametersAsObject))
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
                Api = types.GenerateModule(_apiModuleName)
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
