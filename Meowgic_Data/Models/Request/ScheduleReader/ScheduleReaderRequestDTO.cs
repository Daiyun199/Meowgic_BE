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
        public DateTime DayOfWeek { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public bool IsBooked { get; set; }

        //[Required]
        //public string OrderDetailId { get; set; }
    }
}
