﻿using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface ICardService
    {
        Task<Card> CreateCardAsync(CardRequest card);
        Task<Card?> GetCardByIdAsync(string id);
        Task<IEnumerable<Card>> GetAllCardsAsync();
        Task<Card?> UpdateCardAsync(string id, CardRequest card);
        Task<bool> DeleteCardAsync(string id);
    }
}
