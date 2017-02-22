using CLAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTypeS
{
    internal class Commands
    {
        [Empty, Help]
        public static void Help(string help)
        {
            Console.WriteLine(help);
        }

        [Verb(Description = "Generate TS interfaces and API definition files.")]
        public static void GenTS(
            [Description("Path to assembly containing WebApi classes.")] string assemblyPath,
            [Description("Output folder for TS files.")] string outputFolder)
        {
            var gen = new ConsoleGenerator(assemblyPath, outputFolder);
            gen.GenerateAll();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Parser.RunConsole<Commands>(args);
        }
    }
}
