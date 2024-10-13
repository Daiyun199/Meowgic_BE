using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.CardMeaning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardMeaningController(ICardMeaningService cardMeaningService) : Controller
    {
        private readonly ICardMeaningService _cardMeaningService = cardMeaningService;

        // GET: api/CardMeaning/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardMeaningById([FromRoute] string id)
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
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> CreateCardMeaning([FromBody] CardMeaningRequestDTO cardMeaningRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCardMeaning = await _cardMeaningService.CreateCardMeaningAsync(cardMeaningRequest, HttpContext.User);
            return CreatedAtAction(nameof(GetCardMeaningById), new { id = createdCardMeaning.Id }, createdCardMeaning);
        }

        // PUT: api/CardMeaning/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> UpdateCardMeaning([FromRoute]string id, [FromBody] CardMeaningRequestDTO cardMeaningRequest)
        {
            if (!ModelState.IsValid)
        {
                return BadRequest(ModelState);
        }

            var updatedCardMeaning = await _cardMeaningService.UpdateCardMeaningAsync(id, cardMeaningRequest, HttpContext.User);
            if (updatedCardMeaning == null)
        {
                return NotFound();
            }
            return Ok(updatedCardMeaning);
        }

        // DELETE: api/CardMeaning/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> DeleteCardMeaning([FromRoute] string id)
        {
            var result = await _cardMeaningService.DeleteCardMeaningAsync(id, HttpContext.User);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
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
