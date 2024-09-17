﻿using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.ZodiacColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IZodiacColorService
    {
        Task<ZodiacColor> CreateZodiacColorAsync(ZodiacColorRequestDTO zodiacColorDto);

        // Read (Get by ID)
        Task<ZodiacColor?> GetZodiacColorByIdAsync(string id);

        // Read (Get all)
        Task<IEnumerable<ZodiacColor>> GetAllZodiacColorsAsync();

        // Update
        Task<ZodiacColor?> UpdateZodiacColorAsync(string id, ZodiacColorRequestDTO zodiacColorDto);

        // Delete
        Task<bool> DeleteZodiacColorAsync(string id);
        Task<ZodiacColor?> GetZodiacColorByZodiacIdAsync(string id);
    }
}
