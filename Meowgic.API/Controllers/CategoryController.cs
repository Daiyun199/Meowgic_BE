using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IServiceFactory serviceFactory) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<Category>>> GetPagedCategory([FromQuery] QueryPagedCategory query)
        {
            return await _serviceFactory.GetCategoryService().GetPagedCategory(query);
        }
        [HttpPost("create")]
        public async Task<ActionResult<Category>> CreateCategoryt([FromBody] CategoryRequest request)
        {
            await _serviceFactory.GetCategoryService().CreateCategory(request);
            return Ok();
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCategory([FromRoute] string id, [FromBody] CategoryRequest request)
        {
            await _serviceFactory.GetCategoryService().UpdateCategory(id, request);
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCatregory([FromRoute] string id)
        {
            var result = await _serviceFactory.GetCategoryService().DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult<List<CategoryResponse>>> GetAllCategory()
        {
            return await _serviceFactory.GetCategoryService().GetAll();
        }
    }
}
