using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.CardMeaning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardMeaningController : Controller
    {
        private readonly ICardMeaningService _cardMeaningService;

        public CardMeaningController(ICardMeaningService cardMeaningService)
        {
            _cardMeaningService = cardMeaningService;
        }

        // GET: api/CardMeaning/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardMeaningById(string id)
        {
            var cardMeaning = await _cardMeaningService.GetCardMeaningByIdAsync(id);
            if (cardMeaning == null)
            {
                return NotFound();
            }
            return Ok(cardMeaning);
        }

        // GET: api/CardMeaning
        [HttpGet]
        public async Task<IActionResult> GetAllCardMeanings()
        {
            var cardMeanings = await _cardMeaningService.GetAllCardMeaningsAsync();
            return Ok(cardMeanings);
        }

        // POST: api/CardMeaning
        [HttpPost]
        public async Task<IActionResult> CreateCardMeaning([FromBody] CardMeaningRequestDTO cardMeaningRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCardMeaning = await _cardMeaningService.CreateCardMeaningAsync(cardMeaningRequest);
            return CreatedAtAction(nameof(GetCardMeaningById), new { id = createdCardMeaning.Id }, createdCardMeaning);
        }

        // PUT: api/CardMeaning/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCardMeaning(string id, [FromBody] CardMeaningRequestDTO cardMeaningRequest)
        {
            if (!ModelState.IsValid)
        {
                return BadRequest(ModelState);
        }

            var updatedCardMeaning = await _cardMeaningService.UpdateCardMeaningAsync(id, cardMeaningRequest);
            if (updatedCardMeaning == null)
        {
                return NotFound();
            }
            return Ok(updatedCardMeaning);
        }

        // DELETE: api/CardMeaning/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardMeaning(string id)
        {
            var result = await _cardMeaningService.DeleteCardMeaningAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("random")]
        public async Task<IActionResult> GetRandomCardMeanings()
        {
            try
            {
                IEnumerable<CardMeaningResponseDTO> cardMeanings = await _cardMeaningService.GetRandomCardMeaningsAsync();
                return Ok(cardMeanings);
        }
            catch (Exception ex)
        {
                // Xử lý lỗi tùy ý
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}
