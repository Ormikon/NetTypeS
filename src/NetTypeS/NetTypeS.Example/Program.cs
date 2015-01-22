using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using NetTypeS.Elements.Primitives;
using NetTypeS.Example.Classes;
using NetTypeS.Example.Enums;
using NetTypeS.Example.Generics;
using NetTypeS.Example.Interfaces;
using NetTypeS.Interfaces;
using NetTypeS.Utils;

namespace NetTypeS.Example
{
	class Program
	{
		static void Main(string[] args)
		{
            Console.WriteLine(Generator
                .New()
                .Module(b => b.Include<IComplexExample>()).Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Api", b => b.Include<IComplexExample>()).Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Export("", b => b.Include<IComplexExample>()).Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Export("Api", b => b.Include<IComplexExample>()).Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Declare("", b => b.Include<IComplexExample>()).Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Declare("Api", b => b.Include<IComplexExample>()).Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Api", b => b.Include<IComplexExample>())
                .Module("ApiEnums", b =>
                    b.Include<FirstEnum>().Include<SecondEnum>())
                .Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Api", b =>
                    b.Include<IComplexExample>()
                    .Export("Enums", sb =>
                    sb.Include<FirstEnum>().Include<SecondEnum>()))
                .Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Api", b => b.Include<IComplexExample>())
                .Export("Api.Enums", sb =>
                    sb.Include<FirstEnum>().Include<SecondEnum>())
                .Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Api", b => b.Include<IComplexExample>())
                .Module("Api.Enums", sb =>
                    sb.Include<FirstEnum>().Include<SecondEnum>())
                .Generate("Api"));

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Api", b => b.Include<IComplexExample>())
                .Module("Api.Enums", sb =>
                    sb.Include<FirstEnum>().Include<SecondEnum>())
                .Generate("Api.Enums"));

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Api", b => b.Include<IComplexExample>())
                .Export("Api.Enums", sb =>
                    sb.Include<FirstEnum>().Include<SecondEnum>()
                        .ForEnums(et =>
                            Element.New()
                                .AddText("var ")
                                .AddText(StringUtils.ToCamelCase(et.Name))
                                .AddText("Names = ")
                                .AddBlock(et.Values.Select((ev, i) =>
                                    Element.New()
                                        .AddText(ev.ValueAsInt32().ToString(CultureInfo.InvariantCulture))
                                        .AddText(": \"")
                                        .AddText(ev.Name)
                                        .AddText("\"")
                                        .AddIf(() => i != et.Values.Count - 1, e => e.AddText(","))))
                                .AddText(";")
                                .AddLine()
                        ))
                .Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Api", b => b.Include<ComplexExample>().Include<Classes.Example>())
                .Export("Api.Enums", sb =>
                    sb.Include<FirstEnum>().Include<SecondEnum>())
                .Replace<IExample, Classes.Example>()
                .Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Generics", b => b.Include<ComplexGeneric<string, DateTime>>())
                .Generate());

            Console.WriteLine(new string('=', 30));

            Console.WriteLine(Generator
                .New()
                .Module("Generics", b => b.Include<ClassWithGenericProps>())
                .Generate());

            Console.WriteLine(new string('=', 30));

			Console.WriteLine(Generator
                .New(new GeneratorSettings { IncludeInheritedTypes = true, GenerateNumberTypeForDictionaryKeys = true})
				.Module("Api", b => b.Include<ComplexExample>().Include<Classes.Example>())
				.Export("Api.Enums", sb =>
					sb.Include<FirstEnum>().Include<SecondEnum>())
				.Generate());

			Console.ReadKey();
		}
	}
}
