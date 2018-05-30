using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using NetTypeS.WebApi.ApiModels;

namespace NetTypeS.WebApi.Extensions
{
    internal static class ConverterExtension
    {
        public static EndpointInfo ToEndpointInfo(this ApiDescription apiDescription)
        {
            var controllerDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            return new EndpointInfo
            {
                ActionName = controllerDescriptor?.ActionName,
                GeneratedName = NetTypeS.Utils.StringUtils.ToCamelCase(controllerDescriptor?.ActionName),
                ControllerName = controllerDescriptor?.ControllerName,
                HttpMethodName = apiDescription.HttpMethod,
                RelativePath = apiDescription.RelativePath,
                ResponseType = Utils.ReplaceUnsupportedTypesWithAny(controllerDescriptor?.MethodInfo.ReturnType),
                Parameters = apiDescription.ParameterDescriptions.Select(x => x.ToParameterInfo()).ToArray()
            };
        }

        public static ParameterInfo ToParameterInfo(this ApiParameterDescription parameter) =>
            new ParameterInfo
            {
                GeneratedName = NetTypeS.Utils.StringUtils.ToCamelCase(parameter.Name),
                GeneratedType = Utils.ReplaceUnsupportedTypesWithAny(parameter.Type),
                IsQuery = false
            };
    }
}
