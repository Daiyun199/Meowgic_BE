using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public partial class TarotService
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("Account")]
        public string AccountId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ImgUrl { get; set; }

        public double Price { get; set; }

        public double Rate { get; set; }

        [ForeignKey("Promotion")]
        public string? PromotionId { get; set; }

        public virtual Account Account { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = [];

        public virtual Promotion? Promotion { get; set; }
    }

}
