using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Card;
using Meowgic.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface ICardService
    {
        Task<PagedResultResponse<Card>> GetPagedCards(QueryPagedCard request);

        Task<Card> CreateCard(CardRequest request);

        Task UpdateCard(string id, CardRequest request);

        Task<bool> DeleteCardAsync(string id);
        Task<List<CardResponse>> GetAll();
    }
}
