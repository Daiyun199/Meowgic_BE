using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Card;
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
    public class CardService(IUnitOfWork unitOfWork) : ICardService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PagedResultResponse<Card>> GetPagedCards(QueryPagedCard request)
        {
            return (await _unitOfWork.GetCardRepository().GetPagedCard(request)).Adapt<PagedResultResponse<Card>>();
        }

        public async Task<Card> CreateCard(CardRequest request)
        {
            if (await _unitOfWork.GetCardRepository().AnyAsync(s => s.Name == request.Name))
            {
                throw new BadRequestException("This card already exists");
            }

            var card = request.Adapt<Card>();

            await _unitOfWork.GetCardRepository().AddAsync(card);
            await _unitOfWork.SaveChangesAsync();

            return card.Adapt<Card>();
        }

        public async Task UpdateCard(string id, CardRequest request)
        {
            var card = await _unitOfWork.GetCardRepository().FindOneAsync(s => s.Id == id);

            if (card is not null)
            {
                card = request.Adapt<Card>();

                await _unitOfWork.GetCardRepository().UpdateAsync(card);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Card not found");
            }
        }

        public async Task<bool> DeleteCardAsync(string id)
        {
            var card = await _unitOfWork.GetCardRepository().GetByIdAsync(id);
            if (card == null)
            {
                return false;
            }
            await _unitOfWork.GetCardRepository().DeleteAsync(card);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<CardResponse>> GetAll()
        {
            var cards = (await _unitOfWork.GetCardRepository().GetAll());
            if (cards != null)
            {
                return cards.Adapt<List<CardResponse>>();


            }
            throw new NotFoundException("No card was found");
        }
    }
}
