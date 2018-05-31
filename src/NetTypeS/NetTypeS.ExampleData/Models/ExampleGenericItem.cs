namespace NetTypeS.ExampleData.Models
{
    public class ExampleGenericItem<TEntity>
    {
        public TEntity Item { get; set; }
        public TEntity[] Array { get; set; }
    }
}
