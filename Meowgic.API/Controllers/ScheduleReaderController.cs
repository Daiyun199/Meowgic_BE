using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.ScheduleReader;
using Meowgic.Shares.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Reader")] // Yêu cầu người dùng phải được xác thực và có role "Reader"
    public class ScheduleReaderController : ControllerBase
    {
        private readonly IScheduleReaderService _scheduleReaderService;
        private readonly IMapper _mapper;

        public ScheduleReaderController(IScheduleReaderService scheduleReaderService, IMapper mapper)
        {
            _scheduleReaderService = scheduleReaderService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy tất cả các lịch của Reader
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            try
            {
                var schedules = await _scheduleReaderService.GetAllSchedulesAsync();
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy lịch theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(string id)
        {
            try
            {
                var schedule = await _scheduleReaderService.GetScheduleByIdAsync(id);
                if (schedule == null)
                {
                    return NotFound(new { message = "Schedule not found." });
                }
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Tạo lịch mới
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] ScheduleReaderRequestDTO scheduleRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdSchedule = await _scheduleReaderService.CreateScheduleAsync(scheduleRequest);
                return CreatedAtAction(nameof(GetScheduleById), new { id = createdSchedule.Id }, createdSchedule);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật lịch theo ID
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] ScheduleReaderRequestDTO scheduleRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedSchedule = await _scheduleReaderService.UpdateScheduleAsync(id, scheduleRequest);
                return Ok(updatedSchedule);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Xóa lịch theo ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            try
            {
                var result = await _scheduleReaderService.DeleteScheduleAsync(id);
                if (result)
                {
                    return Ok(new { message = "Schedule deleted successfully." });
                }
                return NotFound(new { message = "Schedule not found or could not be deleted." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
