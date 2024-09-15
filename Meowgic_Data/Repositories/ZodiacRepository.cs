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
    public class ZodiacRepository : IZodiacRepository
    {
        private readonly AppDbContext _context;

        public ZodiacRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task<Zodiac> CreateZodiacAsync(Zodiac zodiac)
        {
            _context.Zodiac.Add(zodiac);
            await _context.SaveChangesAsync();
            return zodiac;
        }

        // Read (Get by ID)
        public async Task<Zodiac?> GetZodiacByIdAsync(string id)
        {
            return await _context.Zodiac.FindAsync(id);
        }
        public async Task<Zodiac?> GetZodiacByNameAsync(string name)
        {
            return await _context.Zodiac
                .Where(z => z.Name == name)
                .FirstOrDefaultAsync();
        }



        // Read (Get all)
        public async Task<IEnumerable<Zodiac>> GetAllZodiacsAsync()
        {
            return await _context.Zodiac.ToListAsync();
        }

        // Update
        public async Task<Zodiac?> UpdateZodiacAsync(Zodiac zodiac)
        {
            var existingZodiac = await _context.Zodiac.FindAsync(zodiac.Id);
            if (existingZodiac != null)
            {
                existingZodiac.Name = zodiac.Name;
                existingZodiac.Description = zodiac.Description;
                existingZodiac.ImgLink = zodiac.ImgLink;

                await _context.SaveChangesAsync();
                return existingZodiac;
            }
            return null;
        }

        // Delete
        public async Task<bool> DeleteZodiacAsync(string id)
        {
            var zodiac = await _context.Zodiac.FindAsync(id);
            if (zodiac != null)
            {
                _context.Zodiac.Remove(zodiac);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
