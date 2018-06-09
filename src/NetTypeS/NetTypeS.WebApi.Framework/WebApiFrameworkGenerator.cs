using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using NetTypeS.Attributes;
using NetTypeS.WebApi.Framework.Extensions;
using NetTypeS.WebApi.Models;

namespace NetTypeS.WebApi.Framework
{
    public class WebApiFrameworkGenerator
    {
        private readonly string _promiseType;
        private readonly string _apiModuleName;
        private readonly Func<ApiDescription, bool> _apiFilter;

        public WebApiFrameworkGenerator(
            string promiseType = "Promise",
            string apiModuleName = "apiDefinition",
            Func<ApiDescription, bool> apiFilter = null
        )
        {
            _promiseType = promiseType;
            _apiModuleName = apiModuleName;
            _apiFilter = apiFilter;
        }

        public GeneratedFiles GenerateAll(IApiExplorer explorer, IEnumerable<Type> additionalTypes = null)
        {
            var webApiGenerator = new WebApiGenerator(_promiseType, _apiModuleName);
            var apiDescriptions = explorer.ApiDescriptions
                .Where(api => !api.ActionDescriptor.GetCustomAttributes<NoTypescriptGenerationAttribute>().Any())
                .ToArray();

            if (_apiFilter != null)
                apiDescriptions = apiDescriptions.Where(a => _apiFilter(a)).ToArray();

            var endpoints = apiDescriptions.Select(x => x.ToEndpointInfo()).ToArray();

            return webApiGenerator.GenerateAll(endpoints, additionalTypes);
        }
    }
}
