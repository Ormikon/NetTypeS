# NetTypeS

The packages with classes for generating TypeScript declarations and code by WebApi for .Net Core and .Net Framework.

# Installation

Download and install the package from NuGet:

- [.Net Core generator](https://www.nuget.org/packages/Ormikon.NetTypeS.WebApi.Core):

```powershell
    Install-Package Ormikon.NetTypeS.WebApi.Core
```

- [.Net Framework generator](https://www.nuget.org/packages/Ormikon.NetTypeS.WebApi.Framework):

```powershell
    Install-Package Ormikon.NetTypeS.WebApi.Framework
```

The following targets are available:

- .NET Platform Standard 2.0

# Getting Started

.Net Core usage:

```cs
class Program
{
    static void Main(string[] args)
    {
        var webHost = CoreWebExample.Program.BuildWebHost(args);
        var apiExplorer = webHost.Services.GetService<IApiDescriptionGroupCollectionProvider>();
        var generator = new WebApiCoreGenerator();
        var files = generator.GenerateAll(apiExplorer);
    }
}
```

.Net Framework usage:

```cs
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
    }
}
```

# Examples

.Net Core:

- [.Net Core Examples](https://github.com/Ormikon/NetTypeS/tree/master/src/NetTypeS/NetTypeS.CoreExample);
- [.Net Core WebApi Implementation](https://github.com/Ormikon/NetTypeS/tree/master/src/NetTypeS/NetTypeS.CoreWebExample);

.Net Framework:

- [.Net Framework Examples](https://github.com/Ormikon/NetTypeS/tree/master/src/NetTypeS/NetTypeS.FrameworkExample);
- [.Net Framework WebApi Implementation](https://github.com/Ormikon/NetTypeS/tree/master/src/NetTypeS/NetTypeS.FrameworkWebExample);
