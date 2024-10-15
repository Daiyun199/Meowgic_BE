using Meowgic.Business.Interface;
using Meowgic.Business.Services;
using Meowgic.Data.Models.Response.PayOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOSController(IServiceFactory serviceFactory) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;

        [HttpPost("create")]
        public async Task<CreatePaymentResult> CreatePaymentLink([FromBody]string orderId)
        {
            return await _serviceFactory.GetPayOSService.CreatePaymentLink(orderId);
        }
        [HttpGet("{orderCode}")]
        public async Task<PaymentLinkInformation> GetPaymentLinkInfomation([FromRoute] int orderCode)
        {
            return await _serviceFactory.GetPayOSService.GetPaymentLinkInformation(orderCode);
        }
        [HttpPut("{orderCode}")]
        public async Task<PaymentLinkInformation> CancelOrder([FromRoute] int orderCode)
        {
            return await _serviceFactory.GetPayOSService.CancelOrder(orderCode);
        }
        [HttpPost("payos_transfer_handler")]
        public async Task<IActionResult> PayOSTransferHandler(WebhookType body)
        {
            try
            {
                var result = await _serviceFactory.GetPayOSService.VerifyPaymentWebhookData(body);

                if (result.IsSuccess)
                {
                    return Ok(new { Message = "Webhook process success", OrderId = "OD" + result.Code });
                }

                return BadRequest(new { Message = "Webhook process failed.", OrderId = "OD" + result.Code });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(new { ex.Message });
            }

        }
    }
}
