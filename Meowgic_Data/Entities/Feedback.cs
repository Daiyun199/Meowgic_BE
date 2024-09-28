using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public class Feedback : AbstractEntity
    {

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string content;
        public int rate;
        [ForeignKey("Account")]
        public string AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;
        [ForeignKey("OrderDetail")]
        public string OrderDetailId {  get; set; }

        public virtual OrderDetail OrderDetail { get; set; }


    }
}
