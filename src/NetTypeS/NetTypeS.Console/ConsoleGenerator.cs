using NetTypeS.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace NetTypeS
{
    class ConsoleGenerator
    {
        string sourceAssemblyPath;
        string outputFolder;
        string[] exclude;

        public ConsoleGenerator(string sourceAssemblyPath, string outputFolder, string[] exclude)
        {
            this.sourceAssemblyPath = sourceAssemblyPath;
            this.outputFolder = outputFolder;
            this.exclude = exclude ?? new string[0];
        }

        public void UpdateFile(string fileName, string newContent)
        {
            string path = Path.Combine(outputFolder, fileName);

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
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
            currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromTargetFolder);
            Assembly assembly = Assembly.LoadFrom(sourceAssemblyPath);
            var initializerClass = assembly.GetTypes().Single(t => t.Name == "WebApiConfig");
            initializerClass.GetMethod("Register").Invoke(null, new[] { httpConfig });

            httpConfig.EnsureInitialized();

            var explorer = httpConfig.Services.GetApiExplorer();
            var generator = new WebApiGenerator(
                apiFilter: api => IsSuitableApi(api)
            );
            var files = generator.GenerateAll(explorer);

            UpdateFile("models.ts", files.Models);
            UpdateFile("apiDefinition.ts", files.Api);
        }

        private bool IsSuitableApi(ApiDescription api)
        {
            return !exclude.Contains(api.ActionDescriptor.ControllerDescriptor.ControllerName);
        }

        private Assembly LoadFromTargetFolder(object sender, ResolveEventArgs args)
        {
            //var asm = Assembly.Load(args.Name);

            var name = new AssemblyName(args.Name).Name;

            if (name == "System.Web.Cors")
            {
                name = "System.Web.Http.Cors";
            }

            string folderPath = Path.GetDirectoryName(sourceAssemblyPath);
            string assemblyPath = Path.Combine(folderPath, name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
    }
}
