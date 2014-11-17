using System;
using System.Text;
using NetTypeS.Interfaces;

namespace NetTypeS.Utils
{
	internal class ScriptBuilder : IScriptBuilder
	{
		private readonly bool indent;
		private readonly char indentChar;
		private readonly int indentSize;
		private int indentLevel;
		private readonly StringBuilder builder;
		private bool newLine;
		private readonly Action disposeIndent;

		public ScriptBuilder(bool indent = true, char indentChar = ' ', int indentSize = 4)
		{
			this.indent = indent;
			this.indentChar = indentChar;
			this.indentSize = indentSize;
			indentLevel = 0;
			builder = new StringBuilder();
			newLine = true;
			disposeIndent = () =>
			                {
				                indentLevel--;
				                if (indentLevel < 0) indentLevel = 0;
			                };
		}

		public IDisposable Indent()
		{
			if (indent)
				indentLevel++;
			return new DisposableIndent(disposeIndent);
		}

		public void Append(string str)
		{
			if (newLine)
			{
				builder.Append(indentChar, indentSize*indentLevel);
				newLine = false;
			}
			builder.Append(str);
		}

		public void AppendLine()
		{
			builder.AppendLine();
			newLine = true;
		}

		public void AppendLine(string str)
		{
			if (newLine)
				builder.Append(indentChar, indentSize*indentLevel);
			builder.AppendLine(str);
			newLine = true;
		}

		public override string ToString()
		{
			return builder.ToString();
		}
	}
}