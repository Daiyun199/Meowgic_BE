using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IOrderService
    {
        Task<PagedResultResponse<OrderResponses>> GetPagedOrders(QueryPageOrder request);

        Task<Order> GetOrderDetailsInfoById(string orderId);

        Task<Order> GetCartInfo(ClaimsPrincipal claim);

        Task ConfirmOrder(ClaimsPrincipal claim, string orderId, List<string> serviceId);

        Task CancelOrder(ClaimsPrincipal calim, string orderId);

        Task UpdateOrderDetail(ClaimsPrincipal claim, string orderId, string serviceId);

        Task DeleteServiceFromCart(ClaimsPrincipal claim, string orderId, string serviceId);

        Task DeleteOrder(ClaimsPrincipal claim, string orderId);
    }
}
