using Meowgic.Data.Entities;
using Meowgic.Shares.Enum;
using Meowgic.Shares;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Meowgic.Data.Models.Request.Account
{
    public class UpdateAccount
    {
        public string Name { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime? Dob { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender")]
        public string Gender { get; set; } = null!;

        public string? Phone { get; set; }

        //public IFormFile? Images { get; set; }

        [EnumDataType(typeof(Roles), ErrorMessage = "Invalid role")]
        public string Role { get; set; } = null!;
    }
}
