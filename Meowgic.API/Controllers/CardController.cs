using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Response.Card;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController(IServiceFactory serviceFactory) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<Card>>> GetPagedCard([FromQuery] QueryPagedCard query)
        {
            return await _serviceFactory.GetCardService().GetPagedCards(query);
        }
        [HttpPost("create-card")]
        public async Task<ActionResult<Card>> CreateCard([FromForm] CardRequest request)
        {
            await _serviceFactory.GetCardService().CreateCard(request);
            return Ok();
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCard([FromRoute] string id, [FromBody] CardRequest request)
        {
            await _serviceFactory.GetCardService().UpdateCard(id, request);
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCard([FromRoute] string id)
        {
            var result = await _serviceFactory.GetCardService().DeleteCardAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult<List<CardResponse>>> GetAllCard()
        {
            return await _serviceFactory.GetCardService().GetAll();
        }
    }
}
