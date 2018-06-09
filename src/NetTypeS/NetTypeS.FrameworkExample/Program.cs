using System;
using System.Web.Http;
using NetTypeS.FrameworkWebExample;
using NetTypeS.WebApi.Framework;

namespace NetTypeS.FrameworkExample
{
    class Program
    {
        static void Main()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.EnsureInitialized();
            var explorer = config.Services.GetApiExplorer();
            var generator = new WebApiFrameworkGenerator();
            var generatedResult = generator.GenerateAll(explorer);

            Console.WriteLine("Models:");
            Console.WriteLine(generatedResult.Models);
            Console.WriteLine();
            Console.WriteLine("Api definition:");
            Console.WriteLine(generatedResult.Api);
            Console.ReadKey();
        }
    }
}
