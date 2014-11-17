using System;
using NetTypeS.Example.Interfaces;

namespace NetTypeS.Example.Classes
{
	public class ComplexExample : IComplexExample
	{
		private IExample complex;
		private Uri simple;
		private IExample[] complexCollection;
		private decimal[] simpleCollection;
		private IComplexExample circular;
		private IComplexExample[] circularCollection;

		public IExample Complex
		{
			get { return complex; }
			set { complex = value; }
		}

		public Uri Simple
		{
			get { return simple; }
			set { simple = value; }
		}

		public IExample[] ComplexCollection
		{
			get { return complexCollection; }
			set { complexCollection = value; }
		}

		public decimal[] SimpleCollection
		{
			get { return simpleCollection; }
			set { simpleCollection = value; }
		}

		public IComplexExample Circular
		{
			get { return circular; }
			set { circular = value; }
		}

		public IComplexExample[] CircularCollection
		{
			get { return circularCollection; }
			set { circularCollection = value; }
		}
	}
}
