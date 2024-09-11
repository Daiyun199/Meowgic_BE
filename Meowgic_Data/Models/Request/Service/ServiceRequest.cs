using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Service
{
    public class ServiceRequest
    {
        [Required(ErrorMessage = "Name is required"), MinLength(2), MaxLength(30)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        public IFormFile? Images { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int PromotionId { get; set; }
    }
}
