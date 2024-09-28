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
        Task AddToCart(ClaimsPrincipal claim, string serviceId);

        Task<List<OrderDetailResponse>> GetList();
        Task RemoveFromCart(string userId, string serviceId);
    }
}
