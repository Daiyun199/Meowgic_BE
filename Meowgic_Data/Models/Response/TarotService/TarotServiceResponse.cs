using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.TarotService
{
    public class TarotServiceResponse
    {
        public string Id { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImgUrl { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public string? PromotionId { get; set; }
    }
}
