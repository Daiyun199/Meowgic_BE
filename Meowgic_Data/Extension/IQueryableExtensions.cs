using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Response;
using Meowgic.Shares.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Extension
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResultResponse<T>> ToPagedResultResponseAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        where T : class
        {
            return new PagedResultResponse<T>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
    }
}
