using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.CardMeaning
{
    public class CardMeaningResponseDTO
    {
        public string Id { get; set; } 

        public string CategoryId { get; set; }

        public string CardId { get; set; }

        public string Meaning { get; set; } = null!;

        public string? ReMeaning { get; set; }

        public string CardName { get; set; } = null!;

        public string LinkUrl { get; set; } = null!;

        public string CategoryName { get; set; } = null!;
    }
}
