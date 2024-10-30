using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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

        public async Task<List<OrderDetail>> GetCart(string accountId)
        {
            return await _context.OrderDetails.AsNoTracking().AsSplitQuery()
                .Include(od => od.Service)
                .Include(od => od.ScheduleReader)
                .Include(od => od.Feedback)
                .Where(od => string.IsNullOrEmpty(od.OrderId) && od.CreatedBy == accountId && od.DeletedTime == null)
                .ToListAsync();
        }
        public async Task<OrderDetail?> GetOrderDetailByIdAsync(string detailId)
        {
            return await _context.OrderDetails.AsNoTracking()
                                        .Where(od => od.Id == detailId)
                                        .Include(od => od.Service)
                                        .Include(od => od.ScheduleReader)
                                        .Include(od => od.Feedback)
                                        .AsSplitQuery()
                                        .SingleOrDefaultAsync();
        }

        public async Task<List<OrderDetail>> GetAllOrderDetails()
        {
            return await _context.OrderDetails.AsNoTracking().AsSplitQuery()
            .Include(od => od.Service)
            .Include(od => od.ScheduleReader)
            .Include(od => od.Feedback)
                .ToListAsync();
        }
        public async Task<List<OrderDetail>> GetAllOrderDetailsByOrderId(string orderId)
        {
            return await _context.OrderDetails.AsNoTracking().AsSplitQuery()
            .Include(od => od.Service)
            .Include(od => od.ScheduleReader)
            .Include(od => od.Feedback)
            .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }
    }
}
