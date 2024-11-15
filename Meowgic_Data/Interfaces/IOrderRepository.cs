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
        Task<List<Order>> GetPagedOrders(QueryPageOrder request);
        Task<List<Order>> GetAll();
        Task<int> GetOrdersSize(QueryPageOrder request);
        Task<Order?> GetOrderDetailsInfoById(string orderId);
        Task<Order?> GetCustomerCartInfo(string accountId);
        Task<int> FindEmptyPositionWithBinarySearch(List<Order> list, int low, int high, string entityName, string entityIndex);
    }
}
