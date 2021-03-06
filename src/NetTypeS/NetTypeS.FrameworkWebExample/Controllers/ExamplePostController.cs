﻿using System.Net.Http;
using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<ExampleComplexItem> GetExampleComplexItemAsync(ExampleSimpleItem request)
        {
            var result = new Task<ExampleComplexItem>(() => new ExampleComplexItem());
            return await result;
        }


        [HttpPost]
        public async Task ExampleAsync()
        {
        }

        [HttpPost]
        public async Task<object> GetExampleObjectAsync(ExampleSimpleItem request)
        {
            return await new Task<object>(() => new object());
        }

        [HttpPost]
        public async Task<int> GetExampleSystemAsync(ExampleSimpleItem request)
        {
            return await new Task<int>(() => 1);
        }

        [HttpPost]
        public async Task<int[]> GetExampleSystemArrayAsync(ExampleSimpleItem request)
        {
            return await new Task<int[]>(() => new int[0]);
        }

        [HttpPost]
        public HttpResponseMessage GetExampleResponseMessage(HttpRequestMessage request)
        {
            return new HttpResponseMessage();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetExampleResponseMessageAsync(HttpRequestMessage request)
        {
            return await new Task<HttpResponseMessage>(() => new HttpResponseMessage());
        }
    }
}
