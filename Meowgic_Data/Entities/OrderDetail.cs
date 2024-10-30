using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public partial class OrderDetail : AbstractEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey("Order")]
        public string? OrderId { get; set; }
        [ForeignKey("Service")]
        public string ServiceId { get; set; } = null!;
        [ForeignKey("ScheduleReader")]
        public string ScheduleReaderId { get; set; } = null!;
        public virtual Order? Order { get; set; } = null!;
        public virtual TarotService Service { get; set; } = null!;
        public virtual ScheduleReader ScheduleReader { get; set; } = null!;
        public virtual Feedback? Feedback { get; set; }

    }

}
