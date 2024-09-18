using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.ZodiacColor;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZodiacColorController : ControllerBase
    {
        private readonly IZodiacColorService _zodiacColorService;

        public ZodiacColorController(IZodiacColorService zodiacColorService)
        {
            _zodiacColorService = zodiacColorService;
        }

        // GET: api/ZodiacColor
        [HttpGet]
        public async Task<IActionResult> GetAllZodiacColors()
        {
            var zodiacColors = await _zodiacColorService.GetAllZodiacColorsAsync();
            return Ok(zodiacColors);
        }

        // GET: api/ZodiacColor/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetZodiacColorById(string id)
        {
            var zodiacColor = await _zodiacColorService.GetZodiacColorByIdAsync(id);
            if (zodiacColor == null)
                return NotFound();
            return Ok(zodiacColor);
        }

        // GET: api/ZodiacColor/zodiac/{zodiacId}
        [HttpGet("zodiac/{zodiacId}")]
        public async Task<IActionResult> GetZodiacColorByZodiacId(string zodiacId)
        {
            var zodiacColor = await _zodiacColorService.GetZodiacColorByZodiacIdAsync(zodiacId);
            if (zodiacColor == null)
                return NotFound();
            return Ok(zodiacColor);
        }

        // POST: api/ZodiacColor
        [HttpPost]
        public async Task<IActionResult> CreateZodiacColor([FromBody] ZodiacColorRequestDTO zodiacColorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdZodiacColor = await _zodiacColorService.CreateZodiacColorAsync(zodiacColorDto);
            return CreatedAtAction(nameof(GetZodiacColorById), new { id = createdZodiacColor.Id }, createdZodiacColor);
        }

        // PUT: api/ZodiacColor/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateZodiacColor(string id, [FromBody] ZodiacColorRequestDTO zodiacColorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedZodiacColor = await _zodiacColorService.UpdateZodiacColorAsync(id, zodiacColorDto);
            if (updatedZodiacColor == null)
                return NotFound();

            return Ok(updatedZodiacColor);
        }

        // DELETE: api/ZodiacColor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZodiacColor(string id)
        {
            var success = await _zodiacColorService.DeleteZodiacColorAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
