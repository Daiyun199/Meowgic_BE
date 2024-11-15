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
        [Required(ErrorMessage = "Name is required"), MinLength(2), MaxLength(30)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please input a right email address")]
        public required string Email { get; set; }
        public required DateTime? Dob { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender")]
        [Required]
        public required string Gender { get; set; }

        public string? Phone { get; set; }
    }
}
