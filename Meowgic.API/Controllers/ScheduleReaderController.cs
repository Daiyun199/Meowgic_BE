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
    [Authorize(Roles = "Reader")] // Yêu cầu người dùng phải được xác thực và có role "Reader
    public class ScheduleReaderController(IScheduleReaderService scheduleReaderService, IMapper mapper) : ControllerBase
    {
        private readonly IScheduleReaderService _scheduleReaderService = scheduleReaderService;
        private readonly IMapper _mapper = mapper;

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
                // Chuyển đổi chuỗi thành DateTime
                DateTime dateTime = DateTime.Parse(scheduleRequest.DayOfWeek);
                DateOnly dayOfWeek = DateOnly.FromDateTime(dateTime); 

                        // Chuyển đổi chuỗi thành TimeOnly
                TimeOnly startTime = TimeOnly.Parse(scheduleRequest.StartTime);
                TimeOnly endTime = TimeOnly.Parse(scheduleRequest.EndTime);
                var scheduleRequest2 = new ScheduleRequestDTO2
                {
                    DayOfWeek = dayOfWeek,
                    EndTime = endTime,
                    IsBooked = scheduleRequest.IsBooked,
                    StartTime = startTime,
                };
                var createdSchedule = await _scheduleReaderService.CreateScheduleAsync(scheduleRequest2);
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
                DateTime dateTime = DateTime.Parse(scheduleRequest.DayOfWeek);
                DateOnly dayOfWeek = DateOnly.FromDateTime(dateTime); 

                        // Chuyển đổi chuỗi thành TimeOnly
                TimeOnly startTime = TimeOnly.Parse(scheduleRequest.StartTime);
                TimeOnly endTime = TimeOnly.Parse(scheduleRequest.EndTime);
                var scheduleRequest2 = new ScheduleRequestDTO2
                {
                    DayOfWeek = dayOfWeek,
                    EndTime = endTime,
                    IsBooked = scheduleRequest.IsBooked,
                    StartTime = startTime,
                };


                var updatedSchedule = await _scheduleReaderService.UpdateScheduleAsync(id, scheduleRequest2);
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
        [HttpGet("byDateRangeAndAccount")]
        public async Task<IActionResult> GetSchedulesByDateRangeAndAccountId([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var schedules = await _scheduleReaderService.GetSchedulesByDateRangeAndAccountIdAsync(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));
                return Ok(schedules);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("schedule-not-booked")]
        public async Task<IActionResult> GetSchedulesByDateRangeAndAccountIdAndIsNotBooked([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var schedules = await _scheduleReaderService.GetSchedulesByDateRangeAndAccountIdAndIsBookedAsync(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate),false);
                return Ok(schedules);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("schedule-not-booked-of-reader")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSchedulesByDateRangeAndReaderIdAndIsNotBooked([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string readerId)
        {
            try
            {
                var schedules = await _scheduleReaderService.GetSchedulesByDateRangeAndReaderIdAndIsBookedAsync(readerId,DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate), false);
                return Ok(schedules);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
