using Meowgic.Data.Data;
using Meowgic.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class CardMeaningRepository : ICardMeaningRepository
    {
        private readonly AppDbContext _context;

        public CardMeaningRepository(AppDbContext context)
        {
            _context = context;

        }
    }
}
