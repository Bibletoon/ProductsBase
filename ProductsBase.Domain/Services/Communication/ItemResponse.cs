namespace ProductsBase.Domain.Services.Communication
{
    public class ItemResponse<T> : BaseResponse where T : class
    {
        public T Item;

        public ItemResponse(bool success, string message, T item)
            : base(success, message)
        {
            Item = item;
        }

        /// <summary>
        ///     Creates a success response
        /// </summary>
        /// <param name="Item">Created item</param>
        public ItemResponse(T item)
            : this(true, string.Empty, item)
        {
        }

        /// <summary>
        ///     Creates a error response
        /// </summary>
        /// <param name="message">Error message</param>
        public ItemResponse(string message)
            : this(false, message, null)
        {
        }
    }
}