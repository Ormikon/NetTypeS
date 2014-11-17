namespace NetTypeS.Interfaces
{
	/// <summary>
	/// TypeScript type codes
	/// </summary>
	public enum TypeScriptTypeCode
	{
		/// <summary>
		/// Simple TypeScript element
		/// </summary>
		Simple,

		/// <summary>
		/// Complex TypeScript element (interface)
		/// </summary>
		Complex,

		/// <summary>
		/// TypeScript enumerable
		/// </summary>
		Enum,

		/// <summary>
		/// TypeScript collection
		/// </summary>
		Collection,

		/// <summary>
		/// Nullable type. Especially for type collector
		/// </summary>
		Nullable
	}
}