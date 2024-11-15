using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Account
{
    public class ResetPassword
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }
        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "New password must be at least 8 characters long")]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        public string confirmPassword { get; set; }
        [Required(ErrorMessage = "OTP is required")]
        public string otpResetPassword { get; set; }

    }
}
