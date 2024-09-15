﻿using Meowgic.Business.Interface;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            await _serviceFactory.GetAccountService().UpdateCustomerInfo(id, request);
            return Ok();
        }

        [HttpGet]
        [Route("basic-info/{id}")]
        public async Task<ActionResult<AccountResponse>> GetCustomerBasicInfo([FromRoute] string id)
        {
            return await _serviceFactory.GetAccountService().GetCustomerInfo(id);
        }
    }
}
