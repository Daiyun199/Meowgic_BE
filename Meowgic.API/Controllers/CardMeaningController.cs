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
    public class CardMeaningController(IServiceFactory serviceFactory) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<CardMeaning>>> GetPagedCardMeaning([FromQuery] QueryPagedCardMeaning query)
        {
            return await _serviceFactory.GetCardMeaningService().GetPagedCardMeanings(query);
        }
        [HttpPost("create")]
        public async Task<ActionResult<CardMeaning>> CreateCard([FromBody] CardMeaningRequest request)
        {
            await _serviceFactory.GetCardMeaningService().CreateCardMeaning(request);
            return Ok();
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCardMeaning([FromRoute] string id, [FromBody] CardMeaningRequest request)
        {
            await _serviceFactory.GetCardMeaningService().UpdateCardMeaning(id, request);
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCardMeaning([FromRoute] string id)
        {
            var result = await _serviceFactory.GetCardMeaningService().DeleteCardMeaningAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult<List<CardMeaningResponse>>> GetAllCard()
        {
            return await _serviceFactory.GetCardMeaningService().GetAll();
        }
    }
}
