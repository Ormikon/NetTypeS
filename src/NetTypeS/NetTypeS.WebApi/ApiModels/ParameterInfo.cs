﻿using System;

namespace NetTypeS.WebApi.ApiModels
{
    internal class ParameterInfo
    {
        public string GeneratedName { get; set; }
        public Type GeneratedType { get; set; }
        public bool IsQuery { get; set; }
    }
}
