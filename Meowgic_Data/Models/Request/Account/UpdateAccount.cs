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
        [Required]
        public required string Name { get; set; }

        public DateTime? Dob { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender")]
        public string Gender { get; set; } = null!;

        public string? Phone { get; set; }

    }
}
