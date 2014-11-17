namespace NetTypeS.Interfaces
{
	/// <summary>
	/// Generator formatter settings
	/// </summary>
	public interface IGeneratorFormatSettings
	{
		bool Indent { get; }
		char IndentChar { get; }
		int IndentSize { get; }
	}
}