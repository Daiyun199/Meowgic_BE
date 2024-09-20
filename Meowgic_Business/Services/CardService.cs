using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;


        public CardService(ICardRepository cardRepository, IMapper mapper)
        {
            
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public async Task<Card> CreateCardAsync(CardRequest cardRequest)
        {
            var card = _mapper.Map<Card>(cardRequest);
            return await _cardRepository.CreateCardAsync(card);
        }

        public async Task<Card?> GetCardByIdAsync(string id)
        {
            return await _cardRepository.GetCardByIdAsync(id);
        }

        public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            return await _cardRepository.GetAllCardsAsync();
        }

        public async Task<Card?> UpdateCardAsync(string id, CardRequest cardRequest)
        {
            var card = _mapper.Map<Card>(cardRequest);
            return await _cardRepository.UpdateCardAsync(id, card);
        }

        public async Task<bool> DeleteCardAsync(string id)
        {
            return await _cardRepository.DeleteCardAsync(id);
        }
    }
}
