using NetTypeS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace NetTypeS.WebApi
{
    class ParameterInfo
    {
        public string GeneratedName { get; }
        public Type GeneratedType { get; }
        public bool IsQuery { get; }

        public ParameterInfo(ApiParameterDescription param)
        {
            GeneratedName = NetTypeS.Utils.StringUtils.ToCamelCase(param.Name);
            GeneratedType = Utils.ReplaceUnsupportedTypesWithAny(param.ParameterDescriptor.ParameterType);
            IsQuery = param.Source == ApiParameterSource.FromUri;
        }

        public void RegisterTypes(IGeneratorModule module)
        {
            module.Include(GeneratedType);
        }
    }
}
