using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public  interface IZodiacRepository
    {
      
        Task<Zodiac> CreateZodiacAsync(Zodiac zodiac);

        Task<Zodiac?> GetZodiacByIdAsync(string id);


        Task<IEnumerable<Zodiac>> GetAllZodiacsAsync();

        Task<Zodiac?> UpdateZodiacAsync(Zodiac zodiac);

       
        Task<bool> DeleteZodiacAsync(string id);
        Task<Zodiac?> GetZodiacByNameAsync(string name);
    }
}
