using System.Collections.Generic;

namespace ProductsBase.Api.Resources
{
    public class PageResource<T>
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}