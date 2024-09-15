using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.Zodiac;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZodiacController : Controller
    {
        private IZodiacService _zodiacService;
        
        public ZodiacController(IZodiacService zodiacService)
        {
            _zodiacService = zodiacService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllZodiacs()
        {
            var zodiacs = await _zodiacService.GetAllZodiacsAsync();
            return Ok(zodiacs);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetZodiacById(string id)
        {
            var zodiac = await _zodiacService.GetZodiacByIdAsync(id);
            if (zodiac == null)
                return NotFound();
            return Ok(zodiac);
        }

        [HttpPost]
        public async Task<IActionResult> CreateZodiac([FromBody] ZodiacRequestDTO zodiacDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdZodiac = await _zodiacService.CreateZodiacAsync(zodiacDto);
            return CreatedAtAction(nameof(GetZodiacById), new { id = createdZodiac.Id }, createdZodiac);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateZodiac(string id, [FromBody] ZodiacRequestDTO zodiacDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedZodiac = await _zodiacService.UpdateZodiacAsync(id, zodiacDto);
            if (updatedZodiac == null)
                return NotFound();

            return Ok(updatedZodiac);
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZodiac(string id)
        {
            var success = await _zodiacService.DeleteZodiacAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
