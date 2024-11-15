using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.OrderDetail;
using Meowgic.Data.Models.Response.OrderDetail;
using Meowgic.Data.Models.Response.PayOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IOrderDetailService
    {
        Task<ResultModel> AddToCart(ClaimsPrincipal claim, AddToCartRequest request);        
        Task<ResultModel> GetCart(ClaimsPrincipal claim);
        Task<ResultModel> GetAll();
        Task<ResultModel> GetAllByOrderId(string orderId);
        Task<ResultModel> GetOrderDetailById(string id);
        Task<ResultModel> RemoveFromCart(ClaimsPrincipal claim, string detailId);
        Task<ResultModel> UpdateOrderDetail(ClaimsPrincipal claim, string detailId, UpdateDetailInfor request);
    }
}
