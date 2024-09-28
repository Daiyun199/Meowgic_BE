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
    public class FeedbackRepository :IFeedbackRepository
    {
        private readonly AppDbContext _context;
        public FeedbackRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all feedbacks
        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        // Get feedback by Id
        public async Task<Feedback?> GetByIdAsync(string id)
        {
            return await _context.Feedbacks.Include(f => f.Account)
                                           .Include(f => f.OrderDetail)
                                           .FirstOrDefaultAsync(f => f.Id == id);
        }

        // Get all feedbacks by order detail Id
        public async Task<List<Feedback>> GetAllByServiceId(string serviceId)
        {
            return await _context.Feedbacks.Where(fb => fb.OrderDetail.ServiceId == serviceId).ToListAsync();
        }

        // Create feedback
        public async Task<Feedback> AddAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        // Update feedback
        public async Task<Feedback> UpdateAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        // Delete feedback
        public async Task<bool> DeleteAsync(string id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
