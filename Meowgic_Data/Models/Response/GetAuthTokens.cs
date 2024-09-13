using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response
{
    public class GetAuthTokens
    {
        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }
    }
}
