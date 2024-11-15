using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Meowgic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarotServiceController(IServiceService serviceService) : Controller
    {
        private readonly IServiceService _serviceService = serviceService;

        [HttpPost]
        [Authorize(Policy = "Reader")]
        public async Task<IActionResult> CreateService([FromBody] ServiceRequest request)
        {
          
            var product = await _serviceService.CreateTarotServiceAsync(request, HttpContext.User);
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServicetById([FromRoute] string id)
        {
            var product = await _serviceService.GetTarotServiceByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("Reader/{id}")]
        public async Task<IActionResult> GetServiceByReaderId([FromRoute] string id)
        {
            var product = await _serviceService.GetTarotServiceByAccountIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }
        //[Authorize(Roles = "Reader")]
        [HttpGet]
        public async Task<IActionResult> GetAllService()
        {
            var products = await _serviceService.GetAllTarotServicesAsync();
            return Ok(products);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Reader")]
        public async Task<IActionResult> UpdateService([FromRoute]string id, [FromBody] ServiceRequest request)
        {
            var updatedProduct = await _serviceService.UpdateTarotServiceAsync(id, request, HttpContext.User);
            if (updatedProduct == null) return NotFound();
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Reader")]
        public async Task<IActionResult> DeleteServicet([FromRoute]string id)
        {
            var result = await _serviceService.DeleteTarotServiceAsync(id, HttpContext.User);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
