using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<PagedResultResponse<Order>> GetPagedOrders(QueryPageOrder request);
        Task<Order?> GetOrderDetailsInfoById(string orderId);
        Task<Order?> GetCustomerCartInfo(string accountId);
    }
}
