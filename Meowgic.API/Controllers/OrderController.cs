using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Order;
using Meowgic.Data.Models.Response.OrderDetail;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<Order>> GetCartInfo()
        {
            return await _serviceFactory.GetOrderService.GetCartInfo(HttpContext.User);
        }
        [HttpPost("add-to-cart")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult> AddtoCart([FromBody] string serviceId)
        {

            await _serviceFactory.GetOrderDetailService.AddToCart(HttpContext.User, serviceId);
            return Ok();
        }
        [HttpPost("{orderId}")]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> CreateOrder([FromRoute]string orderId, [FromBody] List<string> shirts)
        {
            await _serviceFactory.GetOrderService.ConfirmOrder(HttpContext.User, orderId, shirts);
            return Ok();
        }
        [HttpPatch("{orderId}")]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> CancelOrder([FromRoute]string orderId)
        {
            await _serviceFactory.GetOrderService.CancelOrder(HttpContext.User, orderId);
            return Ok();
        }
        [HttpPut("{orderId}")]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> UpdateOrderDetail([FromRoute]string orderId, [FromForm] string serviceId)
        {
            await _serviceFactory.GetOrderService.UpdateOrderDetail(HttpContext.User, orderId, serviceId);
            return Ok();
        }
        [HttpDelete("{orderId}/services/{shirtId}")]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> DeleteShirtFromCart([FromRoute]string orderId, string shirtId)
        {
            await _serviceFactory.GetOrderService.DeleteServiceFromCart(HttpContext.User, orderId, shirtId);
            return Ok();
        }
        [HttpDelete("{orderId}")]
        [Authorize(Policy = "Staff")]
        [Authorize(Policy = "Customer")]
        public async Task<IActionResult> DeleteOrder([FromRoute]string orderId)
        {
            await _serviceFactory.GetOrderService.DeleteOrder(HttpContext.User, orderId);
            return NoContent();
        }
    }
}
