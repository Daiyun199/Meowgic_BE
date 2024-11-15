using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.Zodiac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZodiacController(IZodiacService zodiacService) : Controller
    {
        private IZodiacService _zodiacService = zodiacService;

        [HttpGet]
        public async Task<IActionResult> GetAllZodiacs()
        {
            var zodiacs = await _zodiacService.GetAllZodiacsAsync();
            return Ok(zodiacs);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetZodiacById([FromRoute]string id)
        {
            var zodiac = await _zodiacService.GetZodiacByIdAsync(id);
            if (zodiac == null)
                return NotFound();
            return Ok(zodiac);
        }

        [HttpPost]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> CreateZodiac([FromBody] ZodiacRequestDTO zodiacDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdZodiac = await _zodiacService.CreateZodiacAsync(zodiacDto, HttpContext.User);
            return CreatedAtAction(nameof(GetZodiacById), new { id = createdZodiac.Id }, createdZodiac);
        }

     
        [HttpPut("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> UpdateZodiac([FromRoute]string id, [FromBody] ZodiacRequestDTO zodiacDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedZodiac = await _zodiacService.UpdateZodiacAsync(id, zodiacDto, HttpContext.User);
            if (updatedZodiac == null)
                return NotFound();

            return Ok(updatedZodiac);
        }

      
        [HttpDelete("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> DeleteZodiac([FromRoute] string id)
        {
            var success = await _zodiacService.DeleteZodiacAsync(id, HttpContext.User);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
