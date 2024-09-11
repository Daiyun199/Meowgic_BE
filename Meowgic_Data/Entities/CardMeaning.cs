using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public partial class CardMeaning : AbstractEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("Category")]
        public string CategoryId { get; set; }
        [ForeignKey("Card")]
        public string CardId { get; set; }

        public string Meaning { get; set; } = null!;

        public string? ReMeaning { get; set; }

        public virtual Card Card { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;
    }

}
