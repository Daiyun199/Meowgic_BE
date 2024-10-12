using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Microsoft.AspNetCore.Authorization;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController(ICardService cardService) : Controller
    {
        private readonly ICardService _cardService = cardService;

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await _cardService.GetAllCardsAsync();
            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardById([FromRoute] string id)
        {
            var card = await _cardService.GetCardByIdAsync(id);
            if (card == null)
            {
                return NotFound();
        }
            return Ok(card);
        }

        [HttpPost]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> CreateCard([FromBody] CardRequest cardRequest)
        {
            if (!ModelState.IsValid)
        {
                return BadRequest(ModelState);
            }

            var createdCard = await _cardService.CreateCardAsync(cardRequest, HttpContext.User);
            return CreatedAtAction(nameof(GetCardById), new { id = createdCard.Id }, createdCard);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> UpdateCard([FromRoute]string id, [FromBody] CardRequest cardRequest)
        {
            if (!ModelState.IsValid)
        {
                return BadRequest(ModelState);
            }

            var updatedCard = await _cardService.UpdateCardAsync(id, cardRequest, HttpContext.User);
            if (updatedCard == null)
            {
                return NotFound();
            }

            return Ok(updatedCard);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> DeleteCard([FromRoute] string id)
        {
            var success = await _cardService.DeleteCardAsync(id, HttpContext.User);
            if (!success)
        {
                return NotFound();
            }

            return NoContent();
        }
    }
}
