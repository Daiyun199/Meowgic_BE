using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.ZodiacColor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZodiacColorController(IZodiacColorService zodiacColorService) : ControllerBase
    {
        private readonly IZodiacColorService _zodiacColorService = zodiacColorService;

        // GET: api/ZodiacColor
        [HttpGet]
        public async Task<IActionResult> GetAllZodiacColors()
        {
            var zodiacColors = await _zodiacColorService.GetAllZodiacColorsAsync();
            return Ok(zodiacColors);
        }

        // GET: api/ZodiacColor/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetZodiacColorById([FromRoute]string id)
        {
            var zodiacColor = await _zodiacColorService.GetZodiacColorByIdAsync(id);
            if (zodiacColor == null)
                return NotFound();
            return Ok(zodiacColor);
        }

        // GET: api/ZodiacColor/zodiac/{zodiacId}
        [HttpGet("zodiac/{zodiacId}")]
        public async Task<IActionResult> GetZodiacColorByZodiacId([FromRoute]string zodiacId)
        {
            var zodiacColor = await _zodiacColorService.GetZodiacColorByZodiacIdAsync(zodiacId);
            if (zodiacColor == null)
                return NotFound();
            return Ok(zodiacColor);
        }

        // POST: api/ZodiacColor
        [HttpPost]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> CreateZodiacColor([FromBody] ZodiacColorRequestDTO zodiacColorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdZodiacColor = await _zodiacColorService.CreateZodiacColorAsync(zodiacColorDto, HttpContext.User);
            return CreatedAtAction(nameof(GetZodiacColorById), new { id = createdZodiacColor.Id }, createdZodiacColor);
        }

        // PUT: api/ZodiacColor/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> UpdateZodiacColor([FromRoute] string id, [FromBody] ZodiacColorRequestDTO zodiacColorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedZodiacColor = await _zodiacColorService.UpdateZodiacColorAsync(id, zodiacColorDto, HttpContext.User);
            if (updatedZodiacColor == null)
                return NotFound();

            return Ok(updatedZodiacColor);
        }

        // DELETE: api/ZodiacColor/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> DeleteZodiacColor([FromRoute]string id)
        {
            var success = await _zodiacColorService.DeleteZodiacColorAsync(id, HttpContext.User);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
