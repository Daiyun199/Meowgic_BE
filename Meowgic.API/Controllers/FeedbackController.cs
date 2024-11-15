using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.Feedback;
using System.Threading.Tasks;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Yêu cầu người dùng phải được xác thực và có role "Reader"
    public class FeedbackController(IFeedbackService feedbackService) : ControllerBase
    {
        private readonly IFeedbackService _feedbackService = feedbackService;

        /// <summary>
        /// Lấy tất cả feedbacks
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            return Ok(feedbacks);
        }

        /// <summary>
        /// Lấy feedback theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById([FromRoute]string id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            if (feedback == null)
            {
                return NotFound(new { message = "Feedback not found." });
            }
            return Ok(feedback);
        }

        /// <summary>
        /// Lấy feedback theo service ID
        /// </summary>
        [HttpGet("service/{id}")]
        public async Task<IActionResult> GetFeedbackByOrderDetailId([FromRoute]string id)
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksByServiceIdAsync(id);
            if (feedbacks == null)
            {
                return NotFound(new { message = "Not found any feedbacks for this service" });
            }
            return Ok(feedbacks);
        }

        /// <summary>
        /// Tạo feedback mới
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackRequestDTO feedbackRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdFeedback = await _feedbackService.CreateFeedbackAsync(feedbackRequest);
            return CreatedAtAction(nameof(GetFeedbackById), new { id = createdFeedback.Id }, createdFeedback);
        }

        /// <summary>
        /// Cập nhật feedback theo ID
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> UpdateFeedback([FromRoute]string id, [FromBody] FeedbackRequestDTO feedbackRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedFeedback = await _feedbackService.UpdateFeedbackAsync(id, feedbackRequest);
            if (updatedFeedback == null)
            {
                return NotFound(new { message = "Feedback not found." });
            }

            return Ok(updatedFeedback);
        }

        /// <summary>
        /// Xóa feedback theo ID
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> DeleteFeedback(string id)
        {
            var result = await _feedbackService.DeleteFeedbackAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Feedback not found." });
            }
            return Ok();
        }
    }
}
