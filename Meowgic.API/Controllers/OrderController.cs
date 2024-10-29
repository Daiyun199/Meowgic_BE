using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Request.OrderDetail;
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
        [Route("get-cart")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<List<OrderDetailResponse>>> GetList()
        {
            return await _serviceFactory.GetOrderDetailService.GetList(HttpContext.User);
        }
        [HttpGet("order/{id}")]
        public async Task<ActionResult<Order>> GetOrderInfoById([FromRoute]string id)
        {
            return await _serviceFactory.GetOrderService.GetOrderDetailsInfoById(id);
        }
        [HttpGet("order-detail/{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetailInfoById([FromRoute] string id)
        {
            return await _serviceFactory.GetOrderDetailService.GetOrderDetailById(id);
        }

        [HttpPost("add-to-cart")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<OrderDetailResponse>> AddtoCart(string serviceId, string date, string startTime, string endTime)
        {
            var request = new AddToCartRequest
            {
                ServiceId = serviceId,
                Date = DateOnly.Parse(date),
                StartTime = TimeOnly.Parse(startTime),
                EndTime = TimeOnly.Parse(endTime)
            };
            var item = await _serviceFactory.GetOrderDetailService.AddToCart(HttpContext.User, request);
            return Ok(item);
        }
        [HttpPost("booking-order")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<OrderResponses>> CreateOrder(List<BookingRequest> request)
        {
            var item = await _serviceFactory.GetOrderService.BookingOrder(HttpContext.User, request);
            return Ok(item);
        }
        [HttpPatch("canceld-order/{orderId}")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<OrderResponses>> CancelOrder([FromRoute]string orderId)
        {
            var item = await _serviceFactory.GetOrderService.CancelOrder(HttpContext.User, orderId);
            return Ok(item);
        }

        [HttpPut("update-detail-infor/{detailId}")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<OrderDetailResponse>> UpdateOrderDetail([FromRoute] string detailId, UpdateDetailInfor request)
        {
            var item = await _serviceFactory.GetOrderDetailService.UpdateOrderDetail(HttpContext.User, detailId, request);
            return Ok(item);
        }

        [HttpDelete("remove-from-cart/{detailId}")]
        [Authorize(Policy = "Customer")]
        public async Task<ActionResult<OrderDetailResponse>> DeleteOrder([FromRoute]string detailId)
        {
            var item = await _serviceFactory.GetOrderDetailService.RemoveFromCart(HttpContext.User, detailId);
            return Ok(item);
        }
    }
}
