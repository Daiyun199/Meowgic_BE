using Meowgic.Business.Interface;
using Meowgic.Business.Services;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Request.Service;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Category;
using Meowgic.Data.Models.Response.TarotService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController(IServiceFactory serviceFactory) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;


        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<TarotServiceResponse>>> GetPagedService([FromQuery] QueryPagedService query)
        {
            return await _serviceFactory.GetServiceService().GetPagedService(query);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TarotService>> GetDetail([FromRoute] string id)
        {
            return Ok(await _serviceFactory.GetServiceService().GetServiceDetailById(id));
        }

        [HttpPost("create")]
        public async Task<ActionResult<TarotServiceResponse>> CreateService([FromBody] ServiceRequest request)
        {
            return Ok(await _serviceFactory.GetServiceService().CreateService(request));
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult<TarotServiceResponse>> UpdateService([FromRoute] string id, [FromBody] ServiceRequest request)
        {
            return Ok(await _serviceFactory.GetServiceService().UpdateService(id, request));
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteService([FromRoute] string id)
        {
            var result = await _serviceFactory.GetServiceService().DeleteService(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
