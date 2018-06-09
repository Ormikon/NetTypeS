using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using NetTypeS.WebApi.Core;

namespace NetTypeS.CoreExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var webHost = CoreWebExample.Program.BuildWebHost(args);
            var apiExplorer = webHost.Services.GetService<IApiDescriptionGroupCollectionProvider>();
            var generator = new WebApiCoreGenerator();
            var files = generator.GenerateAll(apiExplorer);
            Console.WriteLine("Models:");
            Console.WriteLine(files.Models);
            Console.WriteLine("Api:");
            Console.WriteLine(files.Api);
            Console.ReadKey();
        }
    }
}
