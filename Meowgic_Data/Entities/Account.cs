using Meowgic.Shares.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public partial class Account : IdentityUser
    {
        [Key]
        public string Id { get; set; } =  Guid.NewGuid().ToString();

        public string Email { get; set; } = null!;

        public string? Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateOnly? Dob { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }

        public Roles Role { get; set; } 

        public string Status { get; set; } = null!;

        public double? Rate { get; set; }

        public bool Premium { get; set; }

        public string? ImgUrl { get; set; }
        public string? Description { get; set; }
        public string? bankAccount { get; set; }
        public double? yearsOfExperience { get; set; }
        public string? Specialization {  get; set; }
        public int? countTarot { get; set; } 
        public bool IsDeleted { get; set; } = false;
        public string? otpResetPassword { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("GETDATE()")]
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public DateTime? DeletedTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpdatedTime { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpires { get; set; }
        public bool isConfirmed { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<TarotService> Services { get; set; } = new List<TarotService>();
        public virtual ICollection<ScheduleReader> ScheduleReaders { get; set; } = new List<ScheduleReader>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
