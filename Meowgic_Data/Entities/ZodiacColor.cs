using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public class ZodiacColor : AbstractEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string BasicColor { get; set; }
        [Required]
        public string SignatureColor { get; set; }
        [Required]
        public string AvoidColor { get; set; }
        public string ZodiacId { get; set; }  
        public Zodiac Zodiac { get; set; } 
    }
}
