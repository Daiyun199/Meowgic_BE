using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.OrderDetail
{
    public class AddToCartRequest
    {
        [Required]
        public string ServiceId { get; set; } = null!;
        [Required]
        public string ScheduleReaderId { get; set; } = null!;
    }
}
