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

        public AccountController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCustomerAccountInfo([FromRoute] string id, [FromBody] UpdateAccount request)
        {
            await _serviceFactory.GetAccountService.UpdateCustomerInfo(id, request);
            return Ok();
        }

        [HttpGet]
        [Route("detail-info/{id}")]
        [Authorize]
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
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> ConfirmEmail()
        {
            try
            {
                await _serviceFactory.GetAccountService.ConfirmEmailUser(HttpContext.User);
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
    }
}
