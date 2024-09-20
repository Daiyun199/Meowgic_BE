using Meowgic.Data.Entities;
using Meowgic.Data.Models.Response.CardMeaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface ICardMeaningRepository : IGenericRepository<CardMeaning>
    {
        Task<CardMeaning> CreateCardMeaningAsync(CardMeaning cardMeaning);
        Task<CardMeaningResponseDTO?> GetCardMeaningByIdAsync(string id);
        Task<IEnumerable<CardMeaning>> GetAllCardMeaningsAsync();
        Task<CardMeaning?> UpdateCardMeaningAsync(string id, CardMeaning cardMeaning);
        Task<bool> DeleteCardMeaningAsync(string id);
        Task<IEnumerable<CardMeaning>> GetCardMeaningsByCategoryAsync(string categoryName);
    }
}
