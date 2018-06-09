using System.Web.Http;
using NetTypeS.ExampleData.Models;

namespace NetTypeS.FrameworkWebExample.Controllers
{
    public class ExamplePostController : ApiController
    {
        [HttpPost]
        public ExampleSimpleItem GetExampleSimpleItem()
        {
            return new ExampleSimpleItem();
        }

        [HttpPost]
        public ExampleSimpleItem GetExampleSimpleItemParameters(int id)
        {
            return new ExampleSimpleItem();
        }

        [HttpPost]
        public ExampleComplexItem GetExampleComplexItem()
        {
            return new ExampleComplexItem();
        }

        [HttpPost]
        public ExampleComplexItem GetExampleComplexItemParameter(int id)
        {
            return new ExampleComplexItem();
        }

        [HttpPost]
        public ExampleGenericItem<ExampleComplexItem> GetExampleGenericItem()
        {
            return new ExampleGenericItem<ExampleComplexItem>();
        }
    }
}
