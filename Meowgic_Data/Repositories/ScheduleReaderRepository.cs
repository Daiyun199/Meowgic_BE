using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class ScheduleReaderRepository : IScheduleReaderRepository
    {
        private readonly AppDbContext _context;

        public ScheduleReaderRepository(AppDbContext context) 
        {
            _context = context;
        }
        // Get all schedules
        public async Task<IEnumerable<ScheduleReader>> GetAllAsync()
        {
            return await _context.ScheduleReaders
                                 .Include(sr => sr.Account)
                             
                                 .ToListAsync();
        }

        // Get schedule by Id
        public async Task<ScheduleReader?> GetByIdAsync(string id)
        {
            return await _context.ScheduleReaders
                                 .Include(sr => sr.Account)
                              
                                 .FirstOrDefaultAsync(sr => sr.Id == id);
        }

        // Create schedule
        public async Task<ScheduleReader> AddAsync(ScheduleReader schedule)
        {
            _context.ScheduleReaders.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        // Update schedule
        public async Task<ScheduleReader> UpdateAsync(ScheduleReader schedule)
        {
            _context.ScheduleReaders.Update(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        // Delete schedule
        public async Task<bool> DeleteAsync(string id)
        {
            var schedule = await _context.ScheduleReaders.FindAsync(id);
            if (schedule != null)
            {
                _context.ScheduleReaders.Remove(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<ScheduleReader>> GetSchedulesByDateRangeAndAccountIdAsync(DateOnly startDate, DateOnly endDate, string accountId)
        {
            return await _context.ScheduleReaders
                                 .Include(sr => sr.Account)
                                 .Where(sr => sr.AccountId == accountId && sr.DayOfWeek >= startDate && sr.DayOfWeek <= endDate)
                                 .ToListAsync();
        }
        public async Task<IEnumerable<ScheduleReader>> GetSchedulesByDateRangeAccountIdAndStatusAsync(DateOnly startDate, DateOnly endDate, string accountId,bool isBooked)
        {
            return await _context.ScheduleReaders
                                 .Include(sr => sr.Account)
                                 .Where(sr => sr.AccountId == accountId && sr.DayOfWeek >= startDate && sr.DayOfWeek <= endDate && sr.IsBooked == isBooked)
                                 .ToListAsync();
        }

    }
}
