using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController(IServiceFactory serviceFactory) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<Question>>> GetPagedQuestion([FromQuery] QueryPagedQuestion query)
        {
            return await _serviceFactory.GetQuestionService().GetPagedQuestion(query);
        }
        [HttpPost("create")]
        public async Task<ActionResult<Question>> CreateQuestion([FromBody] QuestionRequest request)
        {
            await _serviceFactory.GetQuestionService().CreateQuestion(request);
            return Ok();
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateQuestion([FromRoute] string id, [FromBody] QuestionRequest request)
        {
            await _serviceFactory.GetQuestionService().UpdateQuestion(id, request);
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] string id)
        {
            var result = await _serviceFactory.GetQuestionService().DeleteQuestion(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult<List<QuestionResponse>>> GetAllQuestion()
        {
            return await _serviceFactory.GetQuestionService().GetAll();
        }
    }
}
