using System;

namespace NetTypeS.Example.Interfaces
{
	public interface IComplexExample
	{
		IExample Complex { get; set; }
		Uri Simple { get; set; }
		IExample[] ComplexCollection { get; set; }
		decimal[] SimpleCollection { get; set; }
		IComplexExample Circular { get; set; }
		IComplexExample[] CircularCollection { get; set; }
	}
}
