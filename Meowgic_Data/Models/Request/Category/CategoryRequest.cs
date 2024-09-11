using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Category
{
    public class CategoryRequest
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name is too long")]
        public required string Name { get; set; }
    }
}
