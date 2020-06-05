using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetTypeS.Attributes;
using NetTypeS.WebApi.Core.Extensions;
using NetTypeS.WebApi.Models;

namespace NetTypeS.WebApi.Core
{
    public class WebApiCoreGenerator
    {
        private readonly string _promiseType;
        private readonly string _apiModuleName;
        private readonly Func<ApiDescription, bool> _apiFilter;

        public WebApiCoreGenerator(
            string promiseType = "Promise",
            string apiModuleName = "apiDefinition",
            Func<ApiDescription, bool> apiFilter = null
        )
        {
            _promiseType = promiseType;
            _apiModuleName = apiModuleName;
            _apiFilter = apiFilter;
        }

        public GeneratedFiles GenerateAll(IApiDescriptionGroupCollectionProvider explorer, IEnumerable<Type> additionalTypes = null)
        {
            UpdateRelativePath(explorer);

            var webApiGenerator = new WebApiGenerator(_promiseType, _apiModuleName);

            var apiDescriptions = explorer.ApiDescriptionGroups.Items.SelectMany(x => x.Items)
                .Where(api => !(api.ActionDescriptor is ControllerActionDescriptor actionDecriptor &&
                                actionDecriptor.MethodInfo.GetCustomAttributes(typeof(NoTypescriptGenerationAttribute)).Any()))
                .ToArray();

            if (_apiFilter != null)
                apiDescriptions = apiDescriptions.Where(a => _apiFilter(a)).ToArray();

            var endpoints = apiDescriptions.Select(x => x.ToEndpointInfo()).ToArray();

            return webApiGenerator.GenerateAll(endpoints, additionalTypes);
        }

        // Append query string param placeholdes to match ASP.NET WebAPI behavior (.NET Core omits them)
        private void UpdateRelativePath(IApiDescriptionGroupCollectionProvider apiExplorer)
        {
            var apiItems = apiExplorer.ApiDescriptionGroups.Items.SelectMany(x => x.Items);

            foreach (var api in apiItems)
            {
                if (api.ParameterDescriptions.Any(x => x.Source == BindingSource.Query))
                {
                    var sb = new StringBuilder("?");
                    foreach (var parameter in api.ParameterDescriptions.Where(x => x.Source == BindingSource.Query))
                    {
                        sb.Append(parameter.Name).Append("={").Append(parameter.Name).Append("}&");
                    }
                    sb.Remove(sb.Length - 1, 1);

                    api.RelativePath += sb.ToString();
                }
            }
        }
    }
}
