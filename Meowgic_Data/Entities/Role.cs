using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meowgic.Shares.Enum;

namespace Meowgic.Data.Entities
{
    public class Role : IdentityRole
    {
        [Key]
        public string Id { get; set; }

      
        [NotMapped]
        public Roles Name { get; set; }

    }
}
