using System.ComponentModel.DataAnnotations;

namespace ProductsBase.Api.Resources
{
    public class RevokeTokenResource
    {
        [Required]
        public string Token { get; set; }
    }
}