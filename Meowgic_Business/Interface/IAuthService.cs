using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IAuthService
    {
        Task<GetAuthTokens> Login(Login loginDto);
        Task Register(Register registerDto);
    }
}
