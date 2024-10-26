using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IServiceRepository
    {
        Task<TarotService> CreateTarotServiceAsync(TarotService tarotService);
        Task<TarotService?> GetTarotServiceByIdAsync(string id);
        Task<IEnumerable<TarotService>> GetAllTarotServicesAsync();
        Task<TarotService?> UpdateTarotServiceAsync(string id, TarotService tarotService);
        Task<bool> DeleteTarotServiceAsync(string id);
        Task<List<TarotService>> GetTarotServicesByAccountIdAsync(string id);

    }
}
