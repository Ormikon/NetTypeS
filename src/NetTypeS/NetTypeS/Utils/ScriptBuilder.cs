using System;
using System.Text;
using NetTypeS.Interfaces;

namespace NetTypeS.Utils
{
    internal class ScriptBuilder : IScriptBuilder
    {
        private readonly bool _indent;
        private readonly char _indentChar;
        private readonly int _indentSize;
        private int _indentLevel;
        private readonly StringBuilder _builder;
        private bool _newLine;
        private readonly Action _disposeIndent;

        public ScriptBuilder(bool indent = true, char indentChar = ' ', int indentSize = 4)
        {
            _indent = indent;
            _indentChar = indentChar;
            _indentSize = indentSize;
            _indentLevel = 0;
            _builder = new StringBuilder();
            _newLine = true;
            _disposeIndent = () =>
                            {
                                _indentLevel--;
                                if (_indentLevel < 0) _indentLevel = 0;
                            };
        }

        public IDisposable Indent()
        {
            if (_indent)
                _indentLevel++;
            return new DisposableIndent(_disposeIndent);
        }

        public void Append(string str)
        {
            if (_newLine)
            {
                _builder.Append(_indentChar, _indentSize * _indentLevel);
                _newLine = false;
            }
            _builder.Append(str);
        }

        public void AppendLine()
        {
            _builder.AppendLine();
            _newLine = true;
        }

        public void AppendLine(string str)
        {
            if (_newLine)
                _builder.Append(_indentChar, _indentSize * _indentLevel);
            _builder.AppendLine(str);
            _newLine = true;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}