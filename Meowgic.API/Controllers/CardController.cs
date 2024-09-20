using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.Card;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : Controller
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await _cardService.GetAllCardsAsync();
            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardById(string id)
        {
            var card = await _cardService.GetCardByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] CardRequest cardRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCard = await _cardService.CreateCardAsync(cardRequest);
            return CreatedAtAction(nameof(GetCardById), new { id = createdCard.Id }, createdCard);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCard(string id, [FromBody] CardRequest cardRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCard = await _cardService.UpdateCardAsync(id, cardRequest);
            if (updatedCard == null)
            {
                return NotFound();
            }

            return Ok(updatedCard);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(string id)
        {
            var success = await _cardService.DeleteCardAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
