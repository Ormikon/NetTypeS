namespace NetTypeS.Example.Generics
{
	public class ComplexGeneric<TFirst, TSecond>
	{
		public Generic<TFirst> First { get; set; }
		public Generic<TSecond> Second { get; set; }
		public ComplexGeneric<TFirst, TSecond> Circular { get; set; }
		public ComplexGeneric<TSecond, TFirst> CircularReverse { get; set; }
		public Generic<ComplexGeneric<Generic<TFirst>, Generic<TSecond>>> SuperComplex { get; set; }
		public Generic<Generic<int>> Defined { get; set; } 
	}
}
