using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Models.Response.Auth;
using Meowgic.Shares.Enum;
using Meowgic.Shares.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IServiceFactory serviceFactory,IEmailService emailService) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;
        private readonly IEmailService _emailService = emailService;

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAccount([FromBody] Register request)
        {
            await _serviceFactory.GetAuthService.Register(request);
            await _emailService.SendConfirmEmailAsync(request.Email);
            return Ok(request);
        }

        [HttpPost("login")]
        public async Task<ActionResult<GetAuthTokens>> Login([FromBody] Login loginDto)
        {
            var user = await _serviceFactory.GetAuthService.Login(loginDto);
            if (user.Status == UserStatus.Unactive.ToString())
            {
                return BadRequest("Your Account have been banned!!!");
            }
            return Created(nameof(Login), user) ;
        }

        [HttpPost("loginWithouPassword")]
        public async Task<ActionResult<GetAuthTokens>> LoginWithoutPassword(string email)
        {
            var user = await _serviceFactory.GetAuthService.LoginWithoutPassword(email);
            if (user.Status == UserStatus.Unactive.ToString())
            {
                return BadRequest("Your Account have been banned!!!");
            }
            return Created(nameof(LoginWithoutPassword), user);
        }

        [HttpGet("who-am-i")]
        [Authorize]
        public async Task<ActionResult<AccountResponse>> WhoAmI()
        {
            return await _serviceFactory.GetAuthService.GetAuthAccountInfo(HttpContext.User);
        }
    }
}
