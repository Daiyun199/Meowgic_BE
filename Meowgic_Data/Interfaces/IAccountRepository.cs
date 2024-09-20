using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<PagedResultResponse<Account>> GetPagedAccount(QueryPagedAccount queryPagedAccount);
        Task<List<Account>> GetAllAcountCustomer();
        Task<Account?> GetCustomerDetailsInfo(string id);
    }
}
