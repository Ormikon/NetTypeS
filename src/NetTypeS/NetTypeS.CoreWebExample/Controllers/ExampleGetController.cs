﻿using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetTypeS.ExampleData.Models;

namespace NetTypeS.CoreWebExample.Controllers
{
    [Produces("application/json")]
    [Route("api/ExampleGet")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ExampleGetController))]
    [ApiController]
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
        [Route("complex/path/{pathParam}/and/{anotherPathParam}")]
        public ExampleComplexItem GetExampleComplexItemParameter(int pathParam, string fromQuery1, string anotherPathParam, string fromQuery2)
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

        [HttpGet]
        public async Task<object> GetExampleObjectAsync(int id)
        {
            return await new Task<object>(() => new object());
        }

        [HttpGet]
        public async Task<int> GetExampleSystemAsync(int id)
        {
            return await new Task<int>(() => 2);
        }

        [HttpGet]
        public async Task<int[]> GetExampleSystemArrayAsync(int id)
        {
            return await new Task<int[]>(() => new int[0]);
        }

        [HttpGet]
        public HttpResponseMessage GetExampleResponseMessage(int id)
        {
            return new HttpResponseMessage();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetExampleResponseMeassageAsync(int id)
        {
            return await new Task<HttpResponseMessage>(() => new HttpResponseMessage());
        }

        [HttpGet]
        public int GetExampleTwoGetParameters([FromQuery]int limit, [FromQuery]int skip = 0)
        {
            return 0;
        }
    }
}