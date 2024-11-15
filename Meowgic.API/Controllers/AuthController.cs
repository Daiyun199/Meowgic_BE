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
            var result = await _serviceFactory.GetAuthService.Register(request);
            if (result == null)
            {
                return BadRequest("Account with email: " + request.Email + " has aldready exist!!!");
            }
            await _emailService.SendConfirmEmailAsync(request.Email);
            return Ok(result);
        }
        [HttpPost("register-without-password")]
        public async Task<ActionResult> RegisterAccountByGG(RegisterByGG request)
        {
            var result = await _serviceFactory.GetAuthService.RegisterByGG(request);
            if (result == null)
            {
                var user = await _serviceFactory.GetAuthService.LoginWithoutPassword(request.Email);
                if (user.Status == UserStatus.Unactive.ToString())
                {
                    return BadRequest("Your Account have been banned!!!");
                }
                return Created(nameof(LoginWithoutPassword), user);
            }
            await _emailService.SendConfirmEmailAsync(request.Email);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<GetAuthTokens>> Login([FromBody] Login loginDto)
        {
            var user = await _serviceFactory.GetAuthService.Login(loginDto);
            if (user == null)
            {
                return BadRequest("Wrong email or password!!!");
            }
            if (user.Status == UserStatus.Unactive.ToString())
            {
                return BadRequest("Your Account have been banned!!!");
            }
            return user ;
        }

        [HttpPost("loginWithouPassword")]
        public async Task<ActionResult<GetAuthTokens>> LoginWithoutPassword(string email)
        {
            var user = await _serviceFactory.GetAuthService.LoginWithoutPassword(email);
            if (user == null)
            {
                return BadRequest("Wrong email or password!!!");
            }
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
            var result = await _serviceFactory.GetAuthService.GetAuthAccountInfo(HttpContext.User);
            if (result == null)
            {
                return BadRequest("Please login");
            }
            return result;
        }
    }
}
