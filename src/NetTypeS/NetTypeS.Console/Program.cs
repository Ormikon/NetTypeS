using CLAP;

namespace NetTypeS.Console
{
    internal class Commands
    {
        [Empty, Help]
        public static void Help(string help)
        {
            System.Console.WriteLine(help);
        }

        [Verb(Description = "Generate TS interfaces and API definition files.")]
        public static void GenTS(
            [System.ComponentModel.Description("Path to assembly containing WebApi classes.")] string assemblyPath,
            [System.ComponentModel.Description("Output folder for TS files.")] string outputFolder,
            [System.ComponentModel.Description("Exclude controller from TS generation")] string[] exclude
        )
        {
            var gen = new ConsoleGenerator(assemblyPath, outputFolder, exclude);
            gen.GenerateAll();
        }
    }


    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.RunConsole<Commands>(args);
        }
    }
}
