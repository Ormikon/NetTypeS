using NetTypeS.Elements.Primitives;
using NetTypeS.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NetTypeS.WebApi
{
    internal class EnumHelper
    {
        public static void GenerateEnumNameLookups(IGeneratorModule m)
        {
            m.ForEnums(et =>
                Element.New()
                    .AddText("export var ")
                    .AddText(NetTypeS.Utils.StringUtils.ToCamelCase(et.Name))
                    .AddText("Names = ")
                    .AddBlock(et.Values.Select((ev, i) =>
                        Element.New()
                            .AddText("\"")
                            .AddText(ev.ValueAsInt64().ToString(CultureInfo.InvariantCulture))
                            .AddText("\"")
                            .AddText(": \"")
                            .AddText(
                                ev.CustomAttributes.OfType<DisplayAttribute>().Select(a => a.Name).SingleOrDefault()
                                ?? PascalCaseToWords(ev.Name)
                            )
                            .AddText("\"")
                            .AddIf(() => i != et.Values.Count - 1, e => e.AddText(","))))
                    .AddText(";")
                    .AddLine()
            );
        }


        private static string PascalCaseToWords(string ident)
        {
            if (ident == null)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            for (var n = 0; n < ident.Length; n++)
            {
                if (n > 0 && Char.IsUpper(ident[n]) && n < ident.Length - 1 && Char.IsLower(ident[n + 1]))
                {
                    sb.Append(' ');
                }
                sb.Append(ident[n]);
            }
            return sb.ToString();
        }

    }
}
