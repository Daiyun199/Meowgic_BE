using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<List<Account>> GetAllAcountCustomer();
        Task<Account?> GetCustomerDetailsInfo(string id);
    }
}
