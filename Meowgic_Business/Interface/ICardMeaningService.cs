using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Response.CardMeaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface ICardMeaningService
    {
        Task<CardMeaningResponseDTO?> CreateCardMeaningAsync(CardMeaningRequestDTO cardMeaningRequest);
        Task<CardMeaningResponseDTO?> GetCardMeaningByIdAsync(string id);
        Task<IEnumerable<CardMeaningResponseDTO?>> GetAllCardMeaningsAsync();
        Task<CardMeaningResponseDTO?> UpdateCardMeaningAsync(string id, CardMeaningRequestDTO cardMeaningRequest);
        Task<bool> DeleteCardMeaningAsync(string id);
        Task<IEnumerable<CardMeaningResponseDTO>> GetRandomCardMeaningsAsync();
    }
}
