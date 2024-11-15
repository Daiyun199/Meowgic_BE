using Meowgic.Data.Entities;
using Meowgic.Data.Models;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Models.Response.PayOS;
using Meowgic.Shares.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IAccountService
    {
        Task UpdateCustomerInfo(string id, UpdateAccount request);
        Task<bool> DeleteAccountAsync(string id);
        Task<ResultModel> GetCustomerInfo(string id);
        Task<PagedResultResponse<AccountResponse>> GetPagedAccounts(QueryPagedAccount request);
        Task<ServiceResult<string>> ConfirmEmailUser(ClaimsPrincipal claim);
        Task<ServiceResult<string>> ResetPasswordAsync(ResetPassword resetPasswordDTO);
        Task<ServiceResult<string>> ConfirmEmailUserProMax(string id);
        Task<string> UpdateProfile(ClaimsPrincipal claims, string imgURl);
        Task<List<AccountResponseWithoutPassword>> GetAccountsByRole(int roleId);
        Task<List<AccountResponseWithoutPassword>> GetAccountByStatus(UserStatus status);
    }
}
