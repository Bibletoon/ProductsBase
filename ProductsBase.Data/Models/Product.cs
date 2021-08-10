namespace ProductsBase.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public int QuantityInPackage  { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}