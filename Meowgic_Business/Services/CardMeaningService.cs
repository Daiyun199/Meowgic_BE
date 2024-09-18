using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.CardMeaning;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class CardMeaningService(IUnitOfWork unitOfWork) : ICardMeaningService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PagedResultResponse<CardMeaning>> GetPagedCardMeanings(QueryPagedCardMeaning request)
        {
            return (await _unitOfWork.GetCardMeaningRepository().GetPagedCardMeaning(request)).Adapt<PagedResultResponse<CardMeaning>>();
        }

        public async Task<CardMeaning> CreateCardMeaning(CardMeaningRequest request)
        {
            if (await _unitOfWork.GetCardMeaningRepository().AnyAsync(s => s.CardId == request.CardId && s.CategoryId == request.CategoryId))
            {
                throw new BadRequestException("This card meaning already exists");
            }

            var card = request.Adapt<CardMeaning>();

            await _unitOfWork.GetCardMeaningRepository().AddAsync(card);
            await _unitOfWork.SaveChangesAsync();

            return card.Adapt<CardMeaning>();
        }

        public async Task UpdateCardMeaning(string id, CardMeaningRequest request)
        {
            var card = await _unitOfWork.GetCardMeaningRepository().FindOneAsync(s => s.Id == id);

            if (card is not null)
            {
                card = request.Adapt<CardMeaning>();

                await _unitOfWork.GetCardMeaningRepository().UpdateAsync(card);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Card not found");
            }
        }

        public async Task<bool> DeleteCardMeaningAsync(string id)
        {
            var card = await _unitOfWork.GetCardMeaningRepository().GetByIdAsync(id);
            if (card == null)
            {
                return false;
            }
            await _unitOfWork.GetCardMeaningRepository().DeleteAsync(card);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<CardMeaningResponse>> GetAll()
        {
            var cards = (await _unitOfWork.GetCardMeaningRepository().GetAll());
            if (cards != null)
            {
                return cards.Adapt<List<CardMeaningResponse>>();


            }
            throw new NotFoundException("No card was found");
        }
    }
}
