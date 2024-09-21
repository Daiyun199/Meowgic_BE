using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Promotion
{
    public class CreatePromotion
    {
        [Required]
        public required string Description { get; set; }

        [Required]
        public double DiscountPercent { get; set; }

        [Required]
        public double MaxDiscount { get; set; }

        [Required]
        public DateTime ExpireTime { get; set; }
        public string? UserId { get; set; }
    }
}
