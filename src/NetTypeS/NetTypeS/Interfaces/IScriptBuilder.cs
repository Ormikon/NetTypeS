using System;

namespace NetTypeS.Interfaces
{
	public interface IScriptBuilder
	{
		IDisposable Indent();
		void Append(string str);
		void AppendLine();
		void AppendLine(string str);
	}
}