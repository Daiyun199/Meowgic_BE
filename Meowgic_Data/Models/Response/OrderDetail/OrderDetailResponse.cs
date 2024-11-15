using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.OrderDetail
{
    public class OrderDetailResponse
    {
        public string Id { get; set; } = null!;
        public string ServiceName { get; set; } = null!;
        public string? OrderId { get; set; }
        public string Date { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public string CreateBy { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
