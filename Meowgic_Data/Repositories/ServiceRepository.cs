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
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task<TarotService> CreateTarotServiceAsync(TarotService tarotService)
        {
            _context.Services.Add(tarotService);
            await _context.SaveChangesAsync();
            return tarotService;
        }

        public async Task<TarotService?> GetTarotServiceByIdAsync(string id)
        {
            return await _context.Services
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<List<TarotService>> GetTarotServicesByAccountIdAsync(string id)
        {
            return await _context.Services
                .Where(t => t.AccountId == id)
                .ToListAsync();
        }


        public async Task<IEnumerable<TarotService>> GetAllTarotServicesAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<TarotService?> UpdateTarotServiceAsync(string id, TarotService tarotService)
        {
            var existingTarotService = await _context.Services.FindAsync(id);

            if (existingTarotService == null) return null;

            _context.Entry(existingTarotService).CurrentValues.SetValues(tarotService);
            await _context.SaveChangesAsync();
            return existingTarotService;
        }

        public async Task<bool> DeleteTarotServiceAsync(string id)
        {
            var tarotService = await _context.Services.FindAsync(id);
            if (tarotService == null) return false;

            _context.Services.Remove(tarotService);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
