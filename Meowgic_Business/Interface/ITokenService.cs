using Meowgic.Shares.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface ITokenService
    {
        string GenerateAccessToken(string accountId, Roles role, string name);

        string GenerateRefreshToken();

    }
}
