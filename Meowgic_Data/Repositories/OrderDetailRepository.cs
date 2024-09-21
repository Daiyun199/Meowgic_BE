using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
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
    }
}
