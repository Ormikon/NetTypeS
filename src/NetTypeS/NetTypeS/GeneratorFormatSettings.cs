using System;
using NetTypeS.Interfaces;

namespace NetTypeS
{
    public class GeneratorFormatSettings : IGeneratorFormatSettings, ICloneable
    {
        private int _indentSize;

        public GeneratorFormatSettings()
        {
            Indent = true;
            IndentChar = ' ';
            _indentSize = 4;
        }

        public bool Indent { get; set; }

        public char IndentChar { get; set; }

        public int IndentSize
        {
            get { return _indentSize; }
            set { _indentSize = value <= 0 ? 1 : value; }
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