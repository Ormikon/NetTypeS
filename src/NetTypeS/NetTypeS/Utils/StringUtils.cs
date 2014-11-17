using System.Linq;

namespace NetTypeS.Utils
{
	internal static class StringUtils
	{
		public static string[] GetLines(this string str)
		{
			return str.Split('\n').Select(l => l.TrimEnd('\r')).ToArray();
		}
	}
}