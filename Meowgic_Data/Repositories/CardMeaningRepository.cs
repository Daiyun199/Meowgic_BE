using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Extension;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class CardMeaningRepository : GenericRepository<CardMeaning>, ICardMeaningRepository
    {
        private readonly AppDbContext _context;

        public CardMeaningRepository(AppDbContext context) : base(context)
        {
            _context = context;

        }
        private Expression<Func<CardMeaning, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "cardId" => card => card.CardId == null ? card.Id : card.CardId, 
                "categoryId" => card => card.CategoryId == null ? card.Id : card.CategoryId,
                _ => card => card.Id,
            };
        }
        public async Task<PagedResultResponse<CardMeaning>> GetPagedCardMeaning(QueryPagedCardMeaning request)
        {
            var query = _context.CardMeanings.AsQueryable();
            query = query.ApplyPagedCardMeaningFilter(request);
            //Sort
            query = request.OrderByDesc ? query.OrderByDescending(GetSortProperty(request.SortColumn))
                                        : query.OrderBy(GetSortProperty(request.SortColumn));
            //Paging
            return await query.ToPagedResultResponseAsync(request.PageNumber, request.PageSize);
        }

        public async Task<CardMeaning?> GetCardMeaningById(string id)
        {
            var card = await _context.CardMeanings.AsNoTracking()
                                            .AsSplitQuery()
                                            .SingleOrDefaultAsync(s => s.Id == id);


            return card;
        }

        public async Task<List<CardMeaning>> GetAll()
        {
            return await _context.CardMeanings.ToListAsync();
        }
        public void Update(CardMeaning card)
        {
            _context.CardMeanings.Update(card);
        }
    }
}
