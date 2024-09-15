using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Zodiac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IZodiacService
    {
       
        Task<Zodiac> CreateZodiacAsync(ZodiacRequestDTO zodiacDto);
        
        Task<Zodiac?> GetZodiacByIdAsync(string id);

       
        Task<IEnumerable<Zodiac>> GetAllZodiacsAsync();

     
        Task<Zodiac?> UpdateZodiacAsync(string id, ZodiacRequestDTO zodiacDto);

       
        Task<bool> DeleteZodiacAsync(string id);
    }
}
