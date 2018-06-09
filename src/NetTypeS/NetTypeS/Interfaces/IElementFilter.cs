namespace NetTypeS.Interfaces
{
    /// <summary>
    /// Interface with element filters
    /// </summary>
    public interface IElementFilter
    {
        bool IsPropertyIncluded(ITypeProperty property);
    }
}