using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Shares.Enum
{
    public enum ErrorType
    {
        NotFound,
        BadRequest,
        Unauthorized,
        InternalServerError,
        ForbiddenMethod
    }
}
