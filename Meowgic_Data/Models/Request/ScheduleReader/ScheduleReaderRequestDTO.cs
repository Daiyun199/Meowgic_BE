using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.ScheduleReader
{
    public class ScheduleReaderRequestDTO
    {
        [Required]
        public string DayOfWeek { get; set; } // Giữ lại dưới dạng chuỗi

        [Required]
        public string StartTime { get; set; } // Giữ lại dưới dạng chuỗi

        [Required]
        public string EndTime { get; set; } // Giữ lại dưới dạng chuỗi

        public bool IsBooked { get; set; }

        //[Required]
        //public string OrderDetailId { get; set; }
    }
}
