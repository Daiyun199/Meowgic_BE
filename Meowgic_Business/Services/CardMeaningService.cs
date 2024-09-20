using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Response.CardMeaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class CardMeaningService : ICardMeaningService
    {
        private readonly ICardMeaningRepository _cardMeaningRepository;
        private readonly IMapper _mapper;

        public CardMeaningService(ICardMeaningRepository cardMeaningRepository, IMapper mapper)
        {
            _cardMeaningRepository = cardMeaningRepository;
            _mapper = mapper;
        }


        public async Task<CardMeaningResponseDTO> CreateCardMeaningAsync(CardMeaningRequestDTO cardMeaningRequest)
        {
            var cardMeaning = _mapper.Map<CardMeaning>(cardMeaningRequest);
            var createdCardMeaning = await _cardMeaningRepository.CreateCardMeaningAsync(cardMeaning);
            return _mapper.Map<CardMeaningResponseDTO>(createdCardMeaning);
        }

        public async Task<CardMeaningResponseDTO?> GetCardMeaningByIdAsync(string id)
        {
            return await _cardMeaningRepository.GetCardMeaningByIdAsync(id);
        }

        public async Task<IEnumerable<CardMeaningResponseDTO>> GetAllCardMeaningsAsync()
        {
            var cardMeanings = await _cardMeaningRepository.GetAllCardMeaningsAsync();
            return cardMeanings.Select(cm => new CardMeaningResponseDTO
            {
                Id = cm.Id,
                CategoryId = cm.CategoryId,
                CardId = cm.CardId,
                Meaning = cm.Meaning,
                ReMeaning = cm.ReMeaning,
                CardName = cm.Card.Name, // assuming 'Name' is the property in 'Card'
                LinkUrl = cm.Card.ImgUrl, // assuming 'LinkUrl' is the property in 'Card'
                CategoryName = cm.Category.Name // assuming 'Name' is the property in 'Category'
            });
        }

        public async Task<CardMeaningResponseDTO?> UpdateCardMeaningAsync(string id, CardMeaningRequestDTO cardMeaningRequest)
        {
            var cardMeaning = _mapper.Map<CardMeaning>(cardMeaningRequest);
            var updatedCardMeaning = await _cardMeaningRepository.UpdateCardMeaningAsync(id, cardMeaning);
            return _mapper.Map<CardMeaningResponseDTO>(updatedCardMeaning);
        }

        public async Task<bool> DeleteCardMeaningAsync(string id)
        {
            return await _cardMeaningRepository.DeleteCardMeaningAsync(id);
        }

        public async Task<IEnumerable<CardMeaningResponseDTO>> GetRandomCardMeaningsAsync()
        {
            // Lấy tất cả CardMeaning có Category là General
            var allCardMeanings = await _cardMeaningRepository.GetCardMeaningsByCategoryAsync("General");

            // Nhóm CardMeaning theo CardId
            var groupedByCard = allCardMeanings
                .GroupBy(cm => cm.CardId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Chọn ngẫu nhiên 1 CardMeaning từ mỗi CardId
            var randomCardMeanings = new List<CardMeaning>();
            var random = new Random();

            foreach (var group in groupedByCard)
            {
                var cardMeanings = group.Value;
                if (cardMeanings.Any())
                {
                    // Chọn ngẫu nhiên 1 CardMeaning từ danh sách
                    var randomCardMeaning = cardMeanings[random.Next(cardMeanings.Count)];
                    randomCardMeanings.Add(randomCardMeaning);
                }
            }

            // Nếu có nhiều hơn 3 CardMeaning, chọn ngẫu nhiên 3 CardMeaning khác nhau
            if (randomCardMeanings.Count > 3)
            {
                randomCardMeanings = randomCardMeanings.OrderBy(_ => random.Next()).Take(3).ToList();
            }

            // Chuyển đổi CardMeaning thành DTO
            var cardMeaningDtos = _mapper.Map<IEnumerable<CardMeaningResponseDTO>>(randomCardMeanings);
            return cardMeaningDtos;
        }
    }
}
