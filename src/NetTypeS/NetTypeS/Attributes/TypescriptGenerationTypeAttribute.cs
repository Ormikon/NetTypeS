using System;

namespace NetTypeS.Attributes
{
    public class TypescriptGenerationTypeAttribute : Attribute
    {
        public TypescriptGenerationTypeAttribute(string tsType)
        {
            GeneratedType = tsType;
        }

        public string GeneratedType {get;}
    }
}
