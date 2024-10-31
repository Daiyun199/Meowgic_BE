using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Order;
using Meowgic.Data.Models.Response.OrderDetail;
using Meowgic.Data.Repositories;
using Meowgic.Shares.Enum;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class OrderService(IUnitOfWork unitOfWork) : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PagedResultResponse<OrderResponses>> GetPagedOrders(QueryPageOrder request)
        {
            var orders = await _unitOfWork.GetOrderRepository.GetPagedOrders(request);
            var totalCount = await _unitOfWork.GetOrderRepository.GetOrdersSize(request);
            var orderResponses = new List<OrderResponses>();
            foreach (var order in orders)
            {
                var orderResponse = new OrderResponses
                {
                    Id = order.Id,
                    CustomerName = order.Account.Name,
                    TotalPrice = order.TotalPrice,
                    OrderDate = order.OrderDate,
                    Status = order.Status
                };
                orderResponses.Add(orderResponse);
            }
            return new PagedResultResponse<OrderResponses>
            {
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = orderResponses
            };
        }

        public async Task<Order> GetOrderDetailsInfoById(string orderId)
        {
            var order = await _unitOfWork.GetOrderRepository.GetOrderDetailsInfoById(orderId);

            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            return order;
        }
        public async Task<OrderResponses> BookingOrder(ClaimsPrincipal claim, List<BookingRequest> detailIds)
        {
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);
            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var orders = await _unitOfWork.GetOrderRepository.GetAllAsync();
            var id = orders.Count > 0 ? int.Parse(orders.Last().Id[2..]) + 1 : 1;

            double totalPrice = 0;
            var order = new Order
            {
                Id = "OD" + id.ToString("D4"),
                AccountId = userId,
                TotalPrice = (decimal)totalPrice,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Unpaid.ToString()
            };
            await _unitOfWork.GetOrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            foreach (var detailId in detailIds)
            {
                var orderDetail = await _unitOfWork.GetOrderDetailRepository.FindOneAsync(od => od.Id == detailId.DetailId);
                orderDetail.OrderId = "OD" + id.ToString("D4");
                await _unitOfWork.GetOrderDetailRepository.UpdateAsync(orderDetail);

                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);
                if (schedule.IsBooked)
                {
                    throw new BadRequestException("This schedule not availabe");
                }
                schedule.IsBooked = true;
                await _unitOfWork.GetScheduleReaderRepository.UpdateAsync(schedule);

                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(orderDetail.ServiceId);
                totalPrice += service.PromotionId != null ? service.Price * (1 - service.Promotion.DiscountPercent) : service.Price;
            }

            order.TotalPrice = (decimal)totalPrice;

            await _unitOfWork.GetOrderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            var result = new OrderResponses
            {
                Id = order.Id,
                CustomerName = account.Name,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Status = order.Status
            };
            return result;
        }
        public async Task<OrderResponses> CancelOrder(ClaimsPrincipal claim, string orderId)
        {
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);
            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }

            var order = await _unitOfWork.GetOrderRepository.FindOneAsync(o => o.Id == orderId);
            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            if (order.AccountId != userId)
            {
                throw new BadRequestException("The order does not belong to this account.");
            }

            if (order.Status != OrderStatus.Unpaid.ToString())
            {
                throw new BadRequestException("The order status must be 'Unpaid' to cancel.");
            }

            order.Status = OrderStatus.Cancel.ToString();
            
            var orderDetails = await _unitOfWork.GetOrderDetailRepository.GetAllOrderDetailsByOrderId(orderId);
            foreach (var orderDetail in orderDetails)
            {
                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);
                schedule.IsBooked = false;
                await _unitOfWork.GetScheduleReaderRepository.UpdateAsync(schedule);
            }
            await _unitOfWork.SaveChangesAsync();

            var result = new OrderResponses
            {
                Id = order.Id,
                CustomerName = account.Name,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Status = order.Status
            };
            return result;
        }
    }
}
