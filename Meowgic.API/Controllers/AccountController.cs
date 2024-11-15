using Meowgic.Business.Interface;
using Meowgic.Business.Services;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Models.Response.PayOS;
using Meowgic.Shares.Enum;
using Meowgic.Shares.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IServiceFactory serviceFactory, IEmailService emailService) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;
        private readonly IEmailService _emailService = emailService;

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCustomerAccountInfo([FromRoute] string id, [FromBody] UpdateAccount request)
        {
            await _serviceFactory.GetAccountService.UpdateCustomerInfo(id, request);
            return Ok(request);
        }

        [HttpGet]
        [Route("detail-info/{id}")]
        public async Task<ResultModel> GetCustomerBasicInfo([FromRoute] string id )
        {
            return await _serviceFactory.GetAccountService.GetCustomerInfo(id);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<AccountResponse>>> GetPagedAccounts([FromQuery] QueryPagedAccount query)
        {
            return await _serviceFactory.GetAccountService.GetPagedAccounts(query);

        }
        [HttpGet]
        [Route("Reader")]
        public async Task<ActionResult<List<AccountResponseWithoutPassword>>> GetReader()
        {
            var accounts = await _serviceFactory.GetAccountService.GetAccountsByRole(2);
            return Ok(accounts); // Hoặc return accounts; nếu bạn không cần đến Status Code
        }
        [HttpGet]
        [Route("emailConfirm")]
        //[Authorize(Policy = "Customer")]
        public async Task<IActionResult> ConfirmEmail(string id)
        {
            try
            {
                await _serviceFactory.GetAccountService.ConfirmEmailUserProMax(id);
                return Ok($"Success: Confirm Successfully");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("sendOTPResetPassword")]
        public async Task<IActionResult> SendResetPassword([FromBody] string email)
        {
            try
            {
                await _emailService.SendResetPasswordAsync(email);
                return Ok($"Success: Confirm Successfully");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            try
            {
                await _serviceFactory.GetAccountService.ResetPasswordAsync(resetPassword);
                return Ok($"Success: Reset Password Successfully");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [HttpPut("update-profile")]
        [Authorize]
        public async Task<string> UpdateProfile([FromBody] string urlLink)
        {
            return await _serviceFactory.GetAccountService.UpdateProfile(HttpContext.User,urlLink);
        }
        [HttpGet("active")]
        public async Task<ActionResult<List<AccountResponseWithoutPassword>>> GetAccountByStatus()
        {
            try
            {
                var accounts = await _serviceFactory.GetAccountService.GetAccountByStatus(UserStatus.Active);
                return Ok(accounts); // Trả về danh sách tài khoản
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (có thể log và trả về thông báo phù hợp)
                return StatusCode(500, "Lỗi khi lấy tài khoản.");
            }
        }

    }
}
