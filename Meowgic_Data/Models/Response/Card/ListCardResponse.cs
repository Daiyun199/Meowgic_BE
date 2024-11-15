using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.Card
{
    public class ListCardResponse : AbstractEntity
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ImgUrl { get; set; } = null!;
    }
}
