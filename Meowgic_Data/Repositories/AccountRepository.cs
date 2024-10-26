using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Extension;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Shares.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var getAll = await _context.Accounts.AsNoTracking().Where(p => p.Role == Roles.Customer).ToListAsync();
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

        private Expression<Func<Account, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "status" => acc => acc.Status == null ? acc.Id : acc.Status,
                "createddate" => acc => acc.CreatedTime,
                _ => acc => acc.Id,
            };
        }
        public async Task<List<Account>> GetAccountsByRoleAsync(Roles role)
        {
            return await _context.Accounts
                .Where(a => a.Role == role)
                .ToListAsync();
        }
        public async Task<PagedResultResponse<Account>> GetPagedAccount(QueryPagedAccount queryPagedAccount)
        {
            int pageNumber = queryPagedAccount.PageNumber;
            int pageSize = queryPagedAccount.PageSize;
            string sortColumn = queryPagedAccount.SortColumn;
            bool sortByDesc = queryPagedAccount.OrderByDesc;

            var query = _context.Accounts
                    .AsNoTracking()
                    .Where(c => c.Status != UserStatus.Unactive.ToString())
                    .AsQueryable();
            //Sort
            query = sortByDesc ? query.OrderByDescending(GetSortProperty(sortColumn))
                                : query.OrderBy(GetSortProperty(sortColumn));

            //Paging
            return await query.ToPagedResultResponseAsync(pageNumber, pageSize);
        }
        public async Task<List<Account>> GetAccountsByStatus(UserStatus status)
        {
            var activeAccounts = await _context.Accounts
                .AsNoTracking()
                .Where(c => c.Status == status.ToString())
                .ToListAsync();

            return activeAccounts;
        }
    }
}
