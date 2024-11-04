using Meowgic.Shares.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Account
{
    public class RegisterByGG
    {
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public Roles Roles { get; set; }
    }
}
