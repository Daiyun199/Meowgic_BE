using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Card
{
    public class CardRequest
    {
        [Required(ErrorMessage = "Name is required"), MinLength(2), MaxLength(30)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image { get; set; } = null!;

    }
}
