using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Response;
using Meowgic.Shares.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        private readonly ICategoryService _categoryService = categoryService;

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/Category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // POST: api/Category
        [HttpPost]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDTO category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCategory = await _categoryService.CreateCategoryAsync(category, HttpContext.User);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        // PUT: api/Category/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> UpdateCategory([FromRoute]string id, [FromBody] CategoryRequestDTO category)
            {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category, HttpContext.User);
            if (updatedCategory == null)
                return NotFound();

            return Ok(updatedCategory);
            }

        // DELETE: api/Category/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> DeleteCategory([FromRoute] string id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id, HttpContext.User);
            if (!success)
                return NotFound();

            return Ok();
        }

    }
}
