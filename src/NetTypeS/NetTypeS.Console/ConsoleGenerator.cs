using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using NetTypeS.WebApi.Framework;

namespace NetTypeS.Console
{
    internal class ConsoleGenerator
    {
        private readonly string _sourceAssemblyPath;
        private readonly string _outputFolder;
        private readonly string[] _exclude;

        public ConsoleGenerator(string sourceAssemblyPath, string outputFolder, string[] exclude)
        {
            _sourceAssemblyPath = sourceAssemblyPath;
            _outputFolder = outputFolder;
            _exclude = exclude ?? new string[0];
        }

        public void UpdateFile(string fileName, string newContent)
        {
            string path = Path.Combine(_outputFolder, fileName);

            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }

            if (!File.Exists(path) || File.ReadAllText(path) != newContent)
            {
                File.WriteAllText(path, newContent);
            }
        }

        public void GenerateAll()
        {
            var httpConfig = new HttpConfiguration();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += LoadFromTargetFolder;
            Assembly assembly = Assembly.LoadFrom(_sourceAssemblyPath);
            var initializerClass = assembly.GetTypes().Single(t => t.Name == "WebApiConfig");
            initializerClass.GetMethod("Register")?.Invoke(null, new object[] { httpConfig });

            httpConfig.EnsureInitialized();

            var explorer = httpConfig.Services.GetApiExplorer();
            var generator = new WebApiFrameworkGenerator(
                apiFilter: IsSuitableApi
            );
            var files = generator.GenerateAll(explorer);

            UpdateFile("models.ts", files.Models);
            UpdateFile("apiDefinition.ts", files.Api);
        }

        private bool IsSuitableApi(ApiDescription api)
        {
            return !_exclude.Contains(api.ActionDescriptor.ControllerDescriptor.ControllerName);
        }

        private Assembly LoadFromTargetFolder(object sender, ResolveEventArgs args)
        {
            //var asm = Assembly.Load(args.Name);

            var name = new AssemblyName(args.Name).Name;

            if (name == "System.Web.Cors")
            {
                name = "System.Web.Http.Cors";
            }

            var folderPath = Path.GetDirectoryName(_sourceAssemblyPath);
            var assemblyPath = Path.Combine(folderPath, name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
    }
}
