using System;

namespace NetTypeS.Utils
{
	internal class DisposableIndent : IDisposable
	{
		private readonly Action disposeAction;

		public DisposableIndent(Action disposeAction)
		{
			this.disposeAction = disposeAction;
		}

		public void Dispose()
		{
			disposeAction();
		}
	}
}