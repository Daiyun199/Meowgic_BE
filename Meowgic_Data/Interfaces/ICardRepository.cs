using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface ICardRepository
    {
        Task<PagedResultResponse<Card>> GetPagedCard(QueryPagedCard queryPagedClubDto);
        Task<Card?> GetCardDetailById(string id);
        Task<List<Card>> GetAll();
    }
}
