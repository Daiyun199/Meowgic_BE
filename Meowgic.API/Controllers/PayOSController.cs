using Meowgic.Business.Interface;
using Meowgic.Business.Services;
using Meowgic.Data.Models.Response.PayOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOSController(IPayOSService payOSService) : ControllerBase
    {
        private readonly IPayOSService _payOSService = payOSService;

        
        [HttpPost("create")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<ResultModel>> CreatePaymentLink(string orderId)
        {
            var result = await _payOSService.CreatePaymentLink(orderId, HttpContext.User);
            return Ok(result);
        }
        //[HttpGet("{orderCode}")]
        //public async Task<ActionResult<ResultModel>> GetPaymentLinkInfomation([FromRoute] long orderCode)
        //{
        //    var result = await _payOSService.GetPaymentLinkInformation(orderCode);
        //    return Ok(result);
        //}
        //[HttpPut("{orderCode}")]
        //public async Task<ActionResult<ResultModel>> CancelOrder([FromRoute] int orderCode)
        //{
        //    var result = await _payOSService.CancelOrder(orderCode);
        //    return Ok(result);
        //}
        [HttpPost("payos_transfer_handler")]
        public async Task<IActionResult> PayOSTransferHandler(WebhookType body)
        {
                var result = await _payOSService.VerifyPaymentWebhookData(body);

                if (result.IsSuccess)
                {
                    return Ok(new { Message = "Webhook process success", OrderId = "OD" + result.Code });
                } else if (result.Code == -1)
            {
                return Ok(new { Message = "Webhook process failed.", Error = "WebhookType not valid" });
            }

                return Ok(new { Message = "Webhook process failed.", OrderId = "OD" + result.Code });

        }
    }
}
