using System;

namespace NetTypeS.WebApi.Models
{
    public class ParameterInfo
    {
        public string GeneratedName { get; set; }
        public Type GeneratedType { get; set; }
        public bool IsQuery { get; set; }
        public bool IsPath { get; set; }
        public bool IsBody { get; set; }
    }
}
