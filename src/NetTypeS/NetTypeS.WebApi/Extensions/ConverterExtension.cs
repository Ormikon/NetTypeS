using System.Linq;
using System.Web.Http.Description;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using NetTypeS.WebApi.Models;
using ApiDescription = Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription;
using ApiParameterDescription = Microsoft.AspNetCore.Mvc.ApiExplorer.ApiParameterDescription;

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

        public static EndpointInfo ToEndpointInfo(this System.Web.Http.Description.ApiDescription apiDescription) =>
            new EndpointInfo
            {
                ActionName = apiDescription.ActionDescriptor.ActionName,
                GeneratedName = Utils.StringUtils.ToCamelCase(apiDescription.ActionDescriptor.ActionName),
                ControllerName = apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName,
                HttpMethodName = apiDescription.HttpMethod.Method,
                RelativePath = apiDescription.RelativePath,
                ResponseType = Helpers.Utils.ReplaceUnsupportedTypesWithAny(
                    apiDescription.ResponseDescription.ResponseType ?? apiDescription.ResponseDescription.DeclaredType),
                Parameters = apiDescription.ParameterDescriptions.Select(x => x.ToParameterInfo()).ToArray()
            };

        public static ParameterInfo ToParameterInfo(this System.Web.Http.Description.ApiParameterDescription parameter) =>
            new ParameterInfo
            {
                GeneratedName = Utils.StringUtils.ToCamelCase(parameter.Name),
                GeneratedType = Helpers.Utils.ReplaceUnsupportedTypesWithAny(parameter.ParameterDescriptor.ParameterType),
                IsQuery = parameter.Source == ApiParameterSource.FromUri
            };
    }
}
