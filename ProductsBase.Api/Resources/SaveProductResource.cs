using System.ComponentModel.DataAnnotations;

namespace ProductsBase.Api.Resources
{
    public class SaveProductResource
    {
        [Required] [MaxLength(30)] public string Name { get; set; }
        [Required] [Range(0, int.MaxValue)] public int QuantityInPackage { get; set; }
        [Required] [Range(1, 5)] public int UnitOfMeasurement { get; set; }
        [Required] public int CategoryId { get; set; }
    }
}