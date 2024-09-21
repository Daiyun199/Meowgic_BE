using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.Order
{
    public class OrderResponses
    {
        public string Id { get; set; } = null!;
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
