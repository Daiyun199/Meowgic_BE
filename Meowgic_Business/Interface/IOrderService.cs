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

        Task<OrderResponses> BookingOrder(ClaimsPrincipal claim, List<BookingRequest> detailIds);

        Task<OrderResponses> CancelOrder(ClaimsPrincipal calim, string orderId);
    }
}
