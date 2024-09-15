using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Meowgic.Shares.Enum;

namespace Meowgic.Data.Models.Response.Account
{
    public class AccountResponse
    {
        public string Id { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateOnly? Dob { get; set; }

        [EnumDataType(typeof(Gender))]
        public string? Gender { get; set; }

        public string? Phone { get; set; }

        [EnumDataType(typeof(Roles))]
        public string Role { get; set; } = null!;

        [EnumDataType(typeof(UserStatus))]
        public string Status { get; set; } = null!;

        public double? Rate { get; set; }

        public bool Premium { get; set; }

        public string? ImgUrl { get; set; }
        public bool IsDeleted { get; set; }
        public bool isConfirmed { get; set; }
    }
}
