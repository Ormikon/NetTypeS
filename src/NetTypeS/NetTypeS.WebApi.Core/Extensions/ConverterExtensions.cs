using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using NetTypeS.WebApi.Models;

namespace NetTypeS.WebApi.Core.Extensions
{
    internal static class ConverterExtensions
    {
        public static EndpointInfo ToEndpointInfo(this ApiDescription apiDescription)
        {
            var controllerDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            return new EndpointInfo
            {
                ActionName = controllerDescriptor?.ActionName,
                GeneratedName = Utils.StringUtils.ToCamelCase(controllerDescriptor?.ActionName),
                ControllerName = controllerDescriptor?.ControllerName,
                HttpMethodName = apiDescription.HttpMethod,
                RelativePath = apiDescription.RelativePath,
                ResponseType = Helpers.Utils.ReplaceUnsupportedTypesWithAny(controllerDescriptor?.MethodInfo.ReturnType),
                Parameters = apiDescription.ParameterDescriptions.Select(x => x.ToParameterInfo()).ToArray()
            };
        }

        public static ParameterInfo ToParameterInfo(this ApiParameterDescription parameter) =>
            new ParameterInfo
            {
                GeneratedName = Utils.StringUtils.ToCamelCase(parameter.Name),
                GeneratedType = Helpers.Utils.ReplaceUnsupportedTypesWithAny(parameter.Type),
                IsQuery = parameter.Type.GetCustomAttributes(typeof(FromQueryAttribute), true).Any()
            };
    }
}
