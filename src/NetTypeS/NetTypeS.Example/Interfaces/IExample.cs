using System;
using NetTypeS.Example.Enums;

namespace NetTypeS.Example.Interfaces
{
	public interface IExample
	{
		int SimpleInt { get; set; }
		string SimpleString { get; set; }
		bool SimpleBoolean { get; set; }
		DateTime SimpleDate { get; set; }
		FirstEnum FirstEnum { get; set; }
		SecondEnum SecondEnum { get; set; }
	}
}
