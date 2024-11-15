using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.CardMeaning
{
    public class CardMeaningRequestDTO
    {
        [Required]
        public string CategoryId { get; set; }

        [Required]
        public string CardId { get; set; }

        [Required]
        public string Meaning { get; set; }

        public string? ReMeaning { get; set; }
    }
}
