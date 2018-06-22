using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetTypeS.ExampleData.Models;

namespace NetTypeS.CoreWebExample.Controllers
{
    [Produces("application/json")]
    [Route("api/ExamplePost")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ExampleGetController))]
    public class ExamplePostController : Controller
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

        [HttpPost]
        public async Task<ExampleComplexItem> GetExampleComplexItemAsync([FromBody] ExampleSimpleItem request)
        {
            var result = new Task<ExampleComplexItem>(() => new ExampleComplexItem());
            return await result;
        }


        [HttpPost]
        public async Task ExampleAsync()
        {
        }
    }
}