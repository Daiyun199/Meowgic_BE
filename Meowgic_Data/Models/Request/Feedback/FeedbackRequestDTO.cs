using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Feedback
{
    public class FeedbackRequestDTO
    {
        [Required]
        public string content { get; set; }

        [Range(1, 5)]
        public int rate { get; set; }

        [Required]
        public string OrderDetailId { get; set; }
    }
}
