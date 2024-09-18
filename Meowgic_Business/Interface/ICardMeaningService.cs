using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Response;
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
        Task<PagedResultResponse<CardMeaning>> GetPagedCardMeanings(QueryPagedCardMeaning request);

        Task<CardMeaning> CreateCardMeaning(CardMeaningRequest request);

        Task UpdateCardMeaning(string id, CardMeaningRequest request);

        Task<bool> DeleteCardMeaningAsync(string id);
        Task<List<CardMeaningResponse>> GetAll();
    }
}
