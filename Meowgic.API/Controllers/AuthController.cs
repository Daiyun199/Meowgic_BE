using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public AuthController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAccount([FromBody] Register request)
        {
            await _serviceFactory.GetAuthService().Register(request);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<GetAuthTokens>> Login([FromBody] Login loginDto)
        {
            return await _serviceFactory.GetAuthService().Login(loginDto);
        }
    }
}
