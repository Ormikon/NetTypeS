using System;
using NetTypeS.Interfaces;

namespace NetTypeS
{
	public class GeneratorFormatSettings : IGeneratorFormatSettings, ICloneable
	{
		private int indentSize;

		public GeneratorFormatSettings()
		{
			Indent = true;
			IndentChar = ' ';
			indentSize = 4;
		}

		public bool Indent { get; set; }

		public char IndentChar { get; set; }

		public int IndentSize
		{
			get { return indentSize; }
			set { indentSize = value <= 0 ? 1 : value; }
		}

		public GeneratorFormatSettings Clone()
		{
			return new GeneratorFormatSettings
			       {
				       Indent = Indent,
				       IndentChar = IndentChar,
				       IndentSize = IndentSize
			       };
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}
}