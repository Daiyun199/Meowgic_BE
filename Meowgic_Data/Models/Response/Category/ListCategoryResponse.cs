using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.Category
{
    public class ListCategoryResponse : AbstractEntity
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
