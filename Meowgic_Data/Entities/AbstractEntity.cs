using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public abstract class AbstractEntity
    {
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("GETDATE()")]
        public DateTime? CreatedTime { get; set; } = DateTime.Now;

        public DateTime? LastUpdatedTime { get; set; }

        public DateTime? DeletedTime { get; set; }
    }
}
