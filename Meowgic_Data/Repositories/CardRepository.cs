using Meowgic.Data.Data;
using Meowgic.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext context)
        {
            _context = context;

        }
    }
}
