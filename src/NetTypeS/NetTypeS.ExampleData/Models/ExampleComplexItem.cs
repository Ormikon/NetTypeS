using System;
using System.Collections.Generic;
using System.Text;

namespace NetTypeS.ExampleData.Models
{
    public class ExampleComplexItem
    {
        public ExampleSimpleItem Item { get; set; }
        public ExampleEnum Enum { get; set; }
        public ExampleSimpleItem[] Array { get; set; }
    }
}
