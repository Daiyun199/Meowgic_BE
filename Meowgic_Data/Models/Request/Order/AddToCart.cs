using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Order
{
    public class AddToCart
    {
        [Required]
        public int ServiceId { get; set; }
    }
}
