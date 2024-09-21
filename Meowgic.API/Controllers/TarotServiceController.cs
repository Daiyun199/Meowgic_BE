using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Meowgic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarotServiceController : Controller
    {
        private readonly IServiceService _serviceService;
      

        public TarotServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
           
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceRequest request)
        {
          
            var product = await _serviceService.CreateTarotServiceAsync(request);
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServicetById(string id)
        {
            var product = await _serviceService.GetTarotServiceByIdAsync(id);
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
        public async Task<IActionResult> UpdateService(string id, [FromBody] ServiceRequest request)
        {
            var updatedProduct = await _serviceService.UpdateTarotServiceAsync(id, request);
            if (updatedProduct == null) return NotFound();
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicet(string id)
        {
            var result = await _serviceService.DeleteTarotServiceAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
