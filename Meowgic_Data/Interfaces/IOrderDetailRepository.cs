using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        Task<List<OrderDetail>> GetCart(string accountId);
        Task<List<OrderDetail>> GetAllOrderDetails();
        Task<List<OrderDetail>> GetAllOrderDetailsByOrderId(string orderId);
        Task<OrderDetail?> GetOrderDetailByIdAsync(string detailId);
    }
}
