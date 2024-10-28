using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.OrderDetail;
using Meowgic.Data.Models.Response.OrderDetail;
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
        Task<OrderDetailResponse> AddToCart(ClaimsPrincipal claim, AddToCartRequest request);        
        Task<List<OrderDetailResponse>> GetList(ClaimsPrincipal claim);
        Task<OrderDetail> GetOrderDetailById(string id);
        Task<OrderDetailResponse> RemoveFromCart(ClaimsPrincipal claim, string detailId);
        Task<OrderDetailResponse> UpdateOrderDetail(ClaimsPrincipal claim, string detailId, UpdateDetailInfor request);
    }
}
