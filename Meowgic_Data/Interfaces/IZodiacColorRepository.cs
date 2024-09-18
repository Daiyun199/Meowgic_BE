using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IZodiacColorRepository
    {
        // Create
        Task<ZodiacColor> CreateZodiacColorAsync(ZodiacColor zodiacColor);

        // Read (Get by ID)
        Task<ZodiacColor?> GetZodiacColorByIdAsync(string id);

        // Read (Get by ZodiacId)
        Task<ZodiacColor?> GetZodiacColorByZodiacIdAsync(string zodiacId);

        // Read (Get all)
        Task<IEnumerable<ZodiacColor>> GetAllZodiacColorsAsync();

        // Update
        Task<ZodiacColor?> UpdateZodiacColorAsync(ZodiacColor zodiacColor);

        // Delete
        Task<bool> DeleteZodiacColorAsync(string id);
    }
}
