using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Account;
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
    public class AccountController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IEmailService _emailService;
        public AccountController(IServiceFactory serviceFactory, IEmailService emailService)
        {
            _serviceFactory = serviceFactory;
            _emailService = emailService;
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCustomerAccountInfo([FromRoute] string id, [FromBody] UpdateAccount request)
        {
            await _serviceFactory.GetAccountService.UpdateCustomerInfo(id, request);
            return Ok();
        }

        [HttpGet]
        [Route("detail-info/{id}")]
        public async Task<ActionResult<Account>> GetCustomerBasicInfo([FromRoute] string id )
        {
            return await _serviceFactory.GetAccountService.GetCustomerInfo(id);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<AccountResponse>>> GetPagedAccounts([FromQuery] QueryPagedAccount query)
        {
            return await _serviceFactory.GetAccountService.GetPagedAccounts(query);
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

    }
}
