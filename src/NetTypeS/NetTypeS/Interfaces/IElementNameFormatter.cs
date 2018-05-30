namespace NetTypeS.Interfaces
{
    /// <summary>
    /// Name format methods group
    /// </summary>
    public interface IElementNameFormatter
    {
        string FormatModuleName(string moduleNme);
        string FormatInterfaceName(string name);
        string FormatEnumName(string name);
        string FormatEnumValueName(string name);
        string FormatPropertyName(string name);
    }
}