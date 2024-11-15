using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Models.Response.Auth;
using System.Security.Claims;

namespace Meowgic.Business.Interface
{
    public interface IAuthService
    {
        Task<GetAuthTokens?> Login(Login loginDto);
        Task<Register?> Register(Register registerDto);
        Task<GetAuthTokens?> RegisterByGG(RegisterByGG email);
        Task<AccountResponse?> GetAuthAccountInfo(ClaimsPrincipal claims);
        Task<GetAuthTokens?> LoginWithoutPassword(string email);

    }
}
