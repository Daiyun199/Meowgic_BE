using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response.Question;
using Meowgic.Data.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using Meowgic.Data.Models.Request.Promotion;
using Meowgic.Data.Models.Response.Promotion;
using System.ComponentModel.DataAnnotations;
using Meowgic.Data.Models.Response.Category;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController(IServiceFactory serviceFactory) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<ListPromotionResponse>>> GetPagedPromotion([FromQuery] QueryPagedPromotion query)
        {
            return await _serviceFactory.GetPromotionService().GetPagedPromotion(query);
        }
        [HttpPost("create")]
        public async Task<ActionResult<CreatePromotion>> CreatePromotion([FromBody] CreatePromotion request)
        {
            return Ok(await _serviceFactory.GetPromotionService().CreatePromotion(request));
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult<CreatePromotion>> UpdatePromotion([FromRoute] string id, [FromBody] CreatePromotion request)
        {
            return Ok(await _serviceFactory.GetPromotionService().UpdatePromotion(id, request));
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePromotion([FromRoute] string id, [FromForm] string userId)
        {
            var result = await _serviceFactory.GetPromotionService().DeletePromotion(id, userId);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult<List<PromotionResponse>>> GetAllPromotion()
        {
            return await _serviceFactory.GetPromotionService().GetAll();
        }
    }
}
