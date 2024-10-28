using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.ScheduleReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IScheduleReaderService
    {
        Task<IEnumerable<ScheduleReader>> GetAllSchedulesAsync();
        Task<ScheduleReader?> GetScheduleByIdAsync(string id);
        Task<ScheduleReader> CreateScheduleAsync(ScheduleRequestDTO2 scheduleRequest);
        Task<ScheduleReader> UpdateScheduleAsync(string id, ScheduleRequestDTO2 scheduleRequest);
        Task<bool> DeleteScheduleAsync(string id);
        Task<IEnumerable<ScheduleReader>> GetSchedulesByDateRangeAndAccountIdAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<ScheduleReader>> GetSchedulesByDateRangeAndAccountIdAndIsBookedAsync(DateOnly startDate, DateOnly endDate,bool isBooked);
        Task<IEnumerable<ScheduleReader>> GetSchedulesByDateRangeAndReaderIdAndIsBookedAsync(string readerID, DateOnly startDate, DateOnly endDate, bool isBooked);
    }
}
