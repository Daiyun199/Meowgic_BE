using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Entities
{
    public partial class Card : AbstractEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();


        public string Name { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        public virtual ICollection<CardMeaning> CardMeanings { get; set; } = new List<CardMeaning>();
    }

}
