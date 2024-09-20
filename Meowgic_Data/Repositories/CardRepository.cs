using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Extension;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Card;
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
    public class CardRepository : GenericRepository<Card>, ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        private Expression<Func<Card, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "name" => card => card.Name == null ? card.Id : card.Name,
                _ => card => card.Id,
            };
        }

        public async Task<PagedResultResponse<Card>> GetPagedCard(QueryPagedCard queryPagedCardDto)
        {
            int pageNumber = queryPagedCardDto.PageNumber;
            int pageSize = queryPagedCardDto.PageSize;
            string sortColumn = queryPagedCardDto.SortColumn;
            bool sortByDesc = queryPagedCardDto.OrderByDesc;

            var query = _context.Cards
                    .AsNoTracking()
                    .Include(s => s.CardMeanings)
                    .AsQueryable();

            //Sort
            query = sortByDesc ? query.OrderByDescending(GetSortProperty(sortColumn))
                                : query.OrderBy(GetSortProperty(sortColumn));


            //Paging
            return await query.ToPagedResultResponseAsync(pageNumber, pageSize);
        }

        public async Task<Card?> GetCardDetailById(string id)
        {
            var club = await _context.Cards.AsNoTracking()
                                            .Include(s => s.CardMeanings)
                                            .SingleOrDefaultAsync(s => s.Id == id);


            return club;
        }

        public async Task<List<Card>> GetAll()
        {
            return await _context.Cards.ToListAsync();

        }
        public async Task<Card> CreateCardAsync(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<Card?> GetCardByIdAsync(string id)
        {
            return await _context.Cards
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            return await _context.Cards
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<Card?> UpdateCardAsync(string id, Card card)
        {
            var existingCard = await _context.Cards.FindAsync(id);
            if (existingCard == null)
            {
                return null;
            }

            existingCard.ImgUrl = card.ImgUrl;
            existingCard.Name = card.Name;

            _context.Cards.Update(existingCard);
            await _context.SaveChangesAsync();
            return existingCard;
        }

        public async Task<bool> DeleteCardAsync(string id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return false;
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
