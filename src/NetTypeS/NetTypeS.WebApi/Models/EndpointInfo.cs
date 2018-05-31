using System;

namespace NetTypeS.WebApi.Models
{
    internal class EndpointInfo
    {
        public string ActionName { get; set; }
        public string GeneratedName { get; set; }
        public string ControllerName { get; set; }
        public string HttpMethodName { get; set; }
        public string RelativePath { get; set; }
        public ParameterInfo[] Parameters { get; set; }
        public Type ResponseType { get; set; }
    }
}
