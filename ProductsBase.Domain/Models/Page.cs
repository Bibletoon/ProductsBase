using System.Collections.Generic;

namespace ProductsBase.Domain.Models
{
    public class Page<T>
    {
        public static int DefaultSize = 10;
        private const int MaxPageSize = 500;
        private int _pageSize;
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? value : MaxPageSize;
        }

        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }

        public Page()
        {
            Items = new List<T>();
        }
    }
}