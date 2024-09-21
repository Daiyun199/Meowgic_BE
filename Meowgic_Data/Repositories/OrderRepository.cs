using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Extension;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Response;
using Meowgic.Shares.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class OrderRepository(AppDbContext context) : GenericRepository<Order>(context), IOrderRepository
    {
        private readonly AppDbContext _context = context;
        private Expression<Func<Order, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "id" => o => o.Id,
                "orderdate" => o => o.OrderDate,
                _ => o => o.Id
            };
        }
        public async Task<PagedResultResponse<Order>> GetPagedOrders(QueryPageOrder request)
        {
            var query = _context.Orders.AsQueryable();


            query = query.ApplyPagedOrdersFilter(request);


            query = request.OrderByDesc ? query.OrderByDescending(GetSortProperty(request.SortColumn))
                                        : query.OrderBy(GetSortProperty(request.SortColumn));


            return await query.ToPagedResultResponseAsync(request.PageNumber, request.PageSize);
        }

        public async Task<Order?> GetOrderDetailsInfoById(string orderId)
        {
            return await _context.Orders.AsNoTracking()
                                        .Where(o => o.Id == orderId)
                                        .Include(o => o.Account)
                                        .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Service)
                                        .AsSplitQuery()
                                        .SingleOrDefaultAsync();
        }
        public async Task<Order?> GetCustomerCartInfo(string accountId)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Where(o => o.AccountId == accountId && o.Status == OrderStatus.Incart.ToString())
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Service)
                        .ThenInclude(s => s.Promotion)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            return order;
        }
    }
}
