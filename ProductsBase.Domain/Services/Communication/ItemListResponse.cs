using System.Collections.Generic;

namespace ProductsBase.Domain.Services.Communication
{
    public class ItemListResponse<T> : BaseResponse
    {
        public IEnumerable<T> Items;

        public ItemListResponse(bool success, string message, IEnumerable<T> items)
            : base(success, message)
        {
            Items = items;
        }

        /// <summary>
        ///     Creates a success response
        /// </summary>
        /// <param name="Item">Created item</param>
        public ItemListResponse(IEnumerable<T> items)
            : this(true, string.Empty, items)
        {
        }

        /// <summary>
        ///     Creates a error response
        /// </summary>
        /// <param name="message">Error message</param>
        public ItemListResponse(string message)
            : this(false, message, null)
        {
        }
    }
}