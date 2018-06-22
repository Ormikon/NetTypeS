using System.Threading.Tasks;
using System.Web.Http;
using NetTypeS.ExampleData.Models;

namespace NetTypeS.FrameworkWebExample.Controllers
{
    public class ExampleGetController : ApiController
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

        [HttpGet]
        public async Task<ExampleComplexItem> GetExampleComplexItemAsync(int id)
        {
            var result = new Task<ExampleComplexItem>(() => new ExampleComplexItem());
            return await result;
        }


        [HttpGet]
        public async Task ExampleAsync()
        {
        }
    }
}
