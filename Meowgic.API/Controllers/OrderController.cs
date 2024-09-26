using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Order;
using Meowgic.Data.Models.Response.OrderDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IServiceFactory serviceFactory) : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory = serviceFactory;
        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<OrderResponses>>> GetOrders([FromQuery] QueryPageOrder request)
        {
            return await _serviceFactory.GetOrderService.GetPagedOrders(request);
        }
        [HttpGet]
        [Route("get-all-orderdetails")]
        public async Task<ActionResult<List<OrderDetailResponse>>> GetList()
        {
            return await _serviceFactory.GetOrderDetailService.GetList();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderDetailsInfoById([FromRoute]string id)
        {
            return await _serviceFactory.GetOrderService.GetOrderDetailsInfoById(id);
        }
        [HttpGet("get-cart")]
        public async Task<ActionResult<Order>> GetCartInfo([FromForm] string userId)
        {
            return await _serviceFactory.GetOrderService.GetCartInfo(userId);
        }
        [HttpPost("add-to-cart")]
        public async Task<ActionResult> AddtoCart([FromBody] string serviceId, string userId)
        {

            await _serviceFactory.GetOrderDetailService.AddToCart(userId, serviceId);
            return Ok();
        }
        [HttpPost("{orderId}")]
        public async Task<IActionResult> CreateOrder([FromRoute]string orderId, [FromBody] List<string> shirts, [FromForm] string userId)
        {
            await _serviceFactory.GetOrderService.ConfirmOrder(userId, orderId, shirts);
            return Ok();
        }
        [HttpPatch("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute]string orderId, [FromForm] string userId)
        {
            await _serviceFactory.GetOrderService.CancelOrder(userId, orderId);
            return Ok();
        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderDetail([FromRoute]string orderId, [FromForm] string userId, string serviceId)
        {
            await _serviceFactory.GetOrderService.UpdateOrderDetail(userId, orderId, serviceId);
            return Ok();
        }
        [HttpDelete("{orderId}/services/{shirtId}")]
        public async Task<IActionResult> DeleteShirtFromCart([FromRoute]string orderId, string shirtId, [FromForm] string userId)
        {
            await _serviceFactory.GetOrderService.DeleteServiceFromCart(userId, orderId, shirtId);
            return Ok();
        }
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder([FromRoute]string orderId, [FromForm] string userId)
        {
            await _serviceFactory.GetOrderService.DeleteOrder(userId, orderId);
            return NoContent();
        }
    }
}
