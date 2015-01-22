using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTypeS.Example.Generics
{
    public class ClassWithGenericProps
    {
        public Generic<string> GenericString { get; set; }
        public Generic<Classes.Example> GenericExample { get; set; }
    }
}
