using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.ScheduleReader
{
    public class ScheduleReaderResponseDTO
    {
        public string Id { get; set; }
        public DateTime DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsBooked { get; set; }
        public string AccountId { get; set; }
        public string OrderDetailId { get; set; }
    }
}
