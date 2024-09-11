using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public partial class Question : AbstractEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; } = null!;
        [ForeignKey("Category")]
        public string? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
    }
}
