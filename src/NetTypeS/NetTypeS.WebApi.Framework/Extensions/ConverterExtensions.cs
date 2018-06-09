using System.Linq;
using System.Web.Http.Description;
using NetTypeS.WebApi.Models;

namespace NetTypeS.WebApi.Framework.Extensions
{
    internal static class ConverterExtensions
    {
        public static EndpointInfo ToEndpointInfo(this ApiDescription apiDescription) =>
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

        public static ParameterInfo ToParameterInfo(this ApiParameterDescription parameter) =>
            new ParameterInfo
            {
                GeneratedName = Utils.StringUtils.ToCamelCase(parameter.Name),
                GeneratedType = Helpers.Utils.ReplaceUnsupportedTypesWithAny(parameter.ParameterDescriptor.ParameterType),
                IsQuery = parameter.Source == ApiParameterSource.FromUri
            };
    }
}
