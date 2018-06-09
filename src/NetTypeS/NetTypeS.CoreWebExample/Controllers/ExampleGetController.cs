using Microsoft.AspNetCore.Mvc;
using NetTypeS.ExampleData.Models;

namespace NetTypeS.CoreWebExample.Controllers
{
    [Produces("application/json")]
    [Route("api/ExampleGet")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ExampleGetController))]
    public class ExampleGetController : Controller
    {
        [HttpGet]
        public ExampleSimpleItem GetExampleSimpleItem()
        {
            return new ExampleSimpleItem();
        }

        [HttpGet]
        public ExampleSimpleItem GetExampleSimpleItemParameters(int id)
        {
            return new ExampleSimpleItem();
        }

        [HttpGet]
        public ExampleComplexItem GetExampleComplexItem()
        {
            return new ExampleComplexItem();
        }

        [HttpGet]
        public ExampleComplexItem GetExampleComplexItemParameter(int id)
        {
            return new ExampleComplexItem();
        }

        [HttpGet]
        public ExampleGenericItem<ExampleComplexItem> GetExampleGenericItem()
        {
            return new ExampleGenericItem<ExampleComplexItem>();
        }
    }
}