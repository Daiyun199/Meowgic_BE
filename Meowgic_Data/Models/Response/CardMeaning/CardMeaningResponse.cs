using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.CardMeaning
{
    public class CardMeaningResponse
    {
        public string Id { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public string CardId { get; set; } = null!;
        public string Meaning { get; set; } = null!;
        public string? ReMeaning { get; set; }
    }
}
