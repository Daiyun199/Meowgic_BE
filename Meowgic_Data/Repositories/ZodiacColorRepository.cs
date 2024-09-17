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
    public class ZodiacColorRepository : IZodiacColorRepository
    {
        private readonly AppDbContext _context;

        public ZodiacColorRepository(AppDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<ZodiacColor> CreateZodiacColorAsync(ZodiacColor zodiacColor)
        {
            _context.ZodiacColor.Add(zodiacColor);
            await _context.SaveChangesAsync();
            return zodiacColor;
        }

        // Read (Get by ID)
        public async Task<ZodiacColor?> GetZodiacColorByIdAsync(string id)
        {
            return await _context.ZodiacColor.FindAsync(id);
        }

        // Read (Get by ZodiacId)
        public async Task<ZodiacColor?> GetZodiacColorByZodiacIdAsync(string zodiacId)
        {
            return await _context.ZodiacColor
                .Where(zc => zc.ZodiacId == zodiacId)
                .FirstOrDefaultAsync();
        }

        // Read (Get all)
        public async Task<IEnumerable<ZodiacColor>> GetAllZodiacColorsAsync()
        {
            return await _context.ZodiacColor.ToListAsync();
        }

        // Update
        public async Task<ZodiacColor?> UpdateZodiacColorAsync(ZodiacColor zodiacColor)
        {
            var existingZodiacColor = await _context.ZodiacColor.FindAsync(zodiacColor.Id);
            if (existingZodiacColor != null)
            {
                existingZodiacColor.AvoidColor = zodiacColor.AvoidColor;
                existingZodiacColor.BasicColor = zodiacColor.BasicColor;
                existingZodiacColor.SignatureColor = zodiacColor.SignatureColor;

                await _context.SaveChangesAsync();
                return existingZodiacColor;
            }
            return null;
        }

        // Delete
        public async Task<bool> DeleteZodiacColorAsync(string id)
        {
            var zodiacColor = await _context.ZodiacColor.FindAsync(id);
            if (zodiacColor != null)
            {
                _context.ZodiacColor.Remove(zodiacColor);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
