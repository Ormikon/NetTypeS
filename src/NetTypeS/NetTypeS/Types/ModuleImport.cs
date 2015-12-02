using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTypeS.Types
{
    public class ModuleImport
    {
        public string[] Bindings { get; set; }
        public string Alias { get; set; }
        public string Module { get; set; }
    }
}
