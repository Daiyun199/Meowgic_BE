using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class OrderDetailRepository(AppDbContext context) : GenericRepository<OrderDetail>(context), IOrderDetailRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<OrderDetail>> GetOrderDetailsAsync(string accountId)
        {
            return await _context.OrderDetails.AsNoTracking().AsSplitQuery()
                .Include(od => od.Service)
                .Where(od => string.IsNullOrEmpty(od.OrderId) && od.CreatedBy == accountId && od.DeletedTime == null)
                .ToListAsync();
        }
    }
}
