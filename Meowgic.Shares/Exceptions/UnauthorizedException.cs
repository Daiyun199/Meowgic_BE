using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meowgic.Shares.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string message) : base (message)
        {
            
        }
    }
}