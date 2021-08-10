using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsBase.Data.Models;

namespace ProductsBase.Data.Utility.Extensions
{
    public static class PaginationExtension
    {
        public static async Task<Page<T>> PaginateAsync<T>(
            this IQueryable<T> query,
            int page,
            int size,
            CancellationToken cancellationToken)
        {
            var paged = new Page<T>();
            paged.CurrentPage = page;
            paged.PageSize = size;

            var totalItemsTask = query.CountAsync(cancellationToken);

            int skip = (page - 1) * size;
            paged.Items = await query
                                .Skip(skip)
                                .Take(size)
                                .ToListAsync(cancellationToken);

            paged.TotalItems = await totalItemsTask;
            paged.TotalPages = (int)Math.Ceiling(paged.TotalItems / (double)size);

            return paged;
        }
    }
}