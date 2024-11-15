using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Order;
using Meowgic.Data.Models.Response.PayOS;
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
        Task<ResultModel> GetPagedOrders(QueryPageOrder request);

        Task<ResultModel> GetAll();

        Task<ResultModel> GetOrderDetailsInfoById(string orderId);

        Task<ResultModel> BookingOrder(ClaimsPrincipal claim, List<string> detailIds);

        Task<ResultModel> CancelOrder(ClaimsPrincipal calim, string orderId);
    }
}
