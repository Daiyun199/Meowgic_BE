using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Account>> GetAllAcountCustomer()
        {
            var getAll = await _context.Accounts.AsNoTracking().Where(p => p.Role == "Customer").ToListAsync();
            return getAll;
        }

        public async Task<Account?> GetCustomerDetailsInfo(string id)
        {
            return await _context.Accounts
                            .AsNoTracking()
                            .Include(a => a.Orders)
                            .ThenInclude(o => o.OrderDetails)
                            .ThenInclude(od => od.Service)
                            .ThenInclude(s => s.Promotion)
                            .AsSplitQuery()
                            .SingleOrDefaultAsync(a => a.Id == id);
        }
    }
}
