using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public partial class Promotion : AbstractEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Description { get; set; } = null!;

        public double DiscountPercent { get; set; }

        public double MaxDiscount { get; set; }

        public DateTime ExpireTime { get; set; }

        public string? Status { get; set; }

        public virtual ICollection<TarotService> Services { get; set; } = new List<TarotService>();
    }

}
