using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IScheduleReaderRepository
    {
        Task<IEnumerable<ScheduleReader>> GetAllAsync();
        Task<ScheduleReader?> GetByIdAsync(string id);
        Task<ScheduleReader> AddAsync(ScheduleReader schedule);
        Task<ScheduleReader> UpdateAsync(ScheduleReader schedule);
        Task<bool> DeleteAsync(string id);
    }
}
