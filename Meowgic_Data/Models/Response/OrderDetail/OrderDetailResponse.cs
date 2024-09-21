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
        public string OrderId { get; set; } = null!;
        public string ServiceId { get; set; } = null!;
        public double Subtotal { get; set; }
    }
}
