using Meowgic.Data.Entities;

using Meowgic.Data.Models.Response.CardMeaning;

using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Response;

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

        //Task<PagedResultResponse<CardMeaning>> GetPagedCardMeaning(QueryPagedCardMeaning queryPagedCardDto);
        //Task<CardMeaning?> GetCardMeaningById(string id);
        //void Update(CardMeaning cardMeaning);
        //Task<List<CardMeaning>> GetAll();

    }
}
