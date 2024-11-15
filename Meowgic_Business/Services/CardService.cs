using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Repositories;
using Meowgic.Shares.Exceptions;
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
        //private readonly IUnitOfWork _unitOfWork;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;


        public CardService(ICardRepository cardRepository, IMapper mapper, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public async Task<Card> CreateCardAsync(CardRequest cardRequest, ClaimsPrincipal claim)
        {
            var accountId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(accountId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var existingCard = _cardRepository.FindAsync(cm => cm.Name == cardRequest.Name);
            if (existingCard is not null)
            {
                throw new BadRequestException("This card has aldready exist!!");
            }
            var card = _mapper.Map<Card>(cardRequest);
            card.CreatedBy = accountId;
            card.CreatedTime = DateTime.Now;
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

        public async Task<Card?> UpdateCardAsync(string id, CardRequest cardRequest, ClaimsPrincipal claim)
        {
            var accountId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(accountId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var existingCard = _cardRepository.GetCardDetailById(id);
            if (existingCard is null)
            {
                throw new BadRequestException("Card not found!!");
            }
            var card = _mapper.Map<Card>(cardRequest);
            card.LastUpdatedBy = accountId;
            card.LastUpdatedTime = DateTime.Now;
            return await _cardRepository.UpdateCardAsync(id, card);
        }

        public async Task<bool> DeleteCardAsync(string id, ClaimsPrincipal claim)
        {
            var accountId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(accountId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var card = await _cardRepository.GetCardByIdAsync(id);
            if (card is null)
            {
                throw new BadRequestException("Card not found");
            }
            card.DeletedTime = DateTime.Now;
            card.DeletedBy = accountId;
            await _cardRepository.UpdateCardAsync(id, card);
            return true;
        }
    }
}
