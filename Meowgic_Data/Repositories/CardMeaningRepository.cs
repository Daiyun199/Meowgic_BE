using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Response.CardMeaning;
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
        public async Task<CardMeaning> CreateCardMeaningAsync(CardMeaning cardMeaning)
        {
            _context.CardMeanings.Add(cardMeaning);
            await _context.SaveChangesAsync();
            return cardMeaning;
        }

        public async Task<CardMeaningResponseDTO?> GetCardMeaningByIdAsync(string id)
        {
            return await _context.CardMeanings
                .Include(cm => cm.Card)
                .Include(cm => cm.Category)
                .Where(cm => cm.Id == id)
                .Select(cm => new CardMeaningResponseDTO
                {
                    Id = cm.Id,
                    CategoryId = cm.CategoryId,
                    CardId = cm.CardId,
                    Meaning = cm.Meaning,
                    ReMeaning = cm.ReMeaning,
                    CardName = cm.Card.Name, // assuming 'Name' is the property in 'Card'
                    LinkUrl = cm.Card.ImgUrl, // assuming 'LinkUrl' is the property in 'Card'
                    CategoryName = cm.Category.Name // assuming 'Name' is the property in 'Category'
                })
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<CardMeaning>> GetAllCardMeaningsAsync()
        {
            return await _context.CardMeanings
                .Include(cm => cm.Card)
                .Include(cm => cm.Category)
                .ToListAsync();
        }

        public async Task<CardMeaning?> UpdateCardMeaningAsync(string id, CardMeaning cardMeaning)
        {
            var existingCardMeaning = await _context.CardMeanings.FindAsync(id);
            if (existingCardMeaning == null)
                return null;

            // Update properties
            existingCardMeaning.CategoryId = cardMeaning.CategoryId;
            existingCardMeaning.CardId = cardMeaning.CardId;
            existingCardMeaning.Meaning = cardMeaning.Meaning;
            existingCardMeaning.ReMeaning = cardMeaning.ReMeaning;

            await _context.SaveChangesAsync();
            return existingCardMeaning;
        }

        public async Task<bool> DeleteCardMeaningAsync(string id)
        {
            var cardMeaning = await _context.CardMeanings.FindAsync(id);
            if (cardMeaning == null)
                return false;

            _context.CardMeanings.Remove(cardMeaning);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<CardMeaning>> GetCardMeaningsByCategoryAsync(string categoryName)
        {
            return await _context.CardMeanings
                .Include(cm => cm.Card)
                .Include(cm => cm.Category)
                .Where(cm => cm.Category.Name == categoryName)
                .ToListAsync();
        }
    }
}
