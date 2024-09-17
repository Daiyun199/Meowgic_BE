using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.ZodiacColor
{
    public class ZodiacColorRequestDTO
    {
        [Required]
        public string BasicColor { get; set; }
        [Required]
        public string SignatureColor { get; set; }
        [Required]
        public string AvoidColor { get; set; }
        public string ZodiacId { get; set; }
    }
}
