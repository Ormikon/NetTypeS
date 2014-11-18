using System;

namespace NetTypeS.Example.Interfaces
{
	interface IInheritedFromComplexExample : IComplexExample
	{
		DateTime? AnotherProperty { get; set; }
	}
}
