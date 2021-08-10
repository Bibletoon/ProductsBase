using System.ComponentModel.DataAnnotations;

namespace ProductsBase.Api.Resources
{
    public class SaveCategoryResource
    {
        [Required] [MaxLength(30)] public string Name { get; set; }
    }
}