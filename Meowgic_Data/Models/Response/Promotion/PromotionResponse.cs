using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.Promotion
{
    public class PromotionResponse
    {
        public string Id { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double DiscountPercent { get; set; }

        public double MaxDiscount { get; set; }

        public DateTime ExpireTime { get; set; }

        public string? Status { get; set; }
    }
}
