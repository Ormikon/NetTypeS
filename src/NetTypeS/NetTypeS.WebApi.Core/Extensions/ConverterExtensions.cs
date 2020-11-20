using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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
                ResponseType = Helpers.UtilsCore.ReplaceUnsupportedTypes(
                    apiDescription.SupportedResponseTypes.SingleOrDefault()?.Type ?? controllerDescriptor?.MethodInfo.ReturnType),
                Parameters = apiDescription.ActionDescriptor.Parameters.Select(x => x.ToParameterInfo()).ToArray()
            };
        }

        public static ParameterInfo ToParameterInfo(this ParameterDescriptor parameter) =>
            new ParameterInfo
            {
                GeneratedName = Utils.StringUtils.ToCamelCase(parameter.Name),
                GeneratedType = Helpers.UtilsCore.ReplaceUnsupportedTypes(parameter.ParameterType),
                IsQuery = parameter.BindingInfo.BindingSource == BindingSource.Query,
                IsPath = parameter.BindingInfo.BindingSource == BindingSource.Path,
                IsBody = parameter.BindingInfo.BindingSource == BindingSource.Body,
            };
    }
}
