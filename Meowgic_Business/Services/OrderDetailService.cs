using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Response.OrderDetail;
using Meowgic.Shares.Enum;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Mapster;
using Azure.Core;
using Meowgic.Data.Repositories;

namespace Meowgic.Business.Services
{
    public class OrderDetailService(IUnitOfWork unitOfWork) : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddToCart(ClaimsPrincipal claim, string serviceId)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var order = await _unitOfWork.GetOrderRepository.FindOneAsync(o => o.AccountId == userId && o.Status == OrderStatus.Incart.ToString());

            if (order is null)
            {
                var orders = await _unitOfWork.GetOrderRepository.GetAllAsync();
                var id = orders.Count + 1;
                if (_unitOfWork.GetOrderRepository.GetByIdAsync($"OD{id.ToString("D4")}") is not null)
                {
                    id = await _unitOfWork.GetOrderRepository.FindEmptyPositionWithBinarySearch(orders, 1, id, "OD", "Id");
                }
                order = new Order
                {
                    Id = "OD" + id.ToString("D4"),
                    AccountId = userId,
                    Status = OrderStatus.Incart.ToString(),
                    TotalPrice = 0,
                    OrderDate = DateTime.Now
                };

                await _unitOfWork.GetOrderRepository.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();
            }

            var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(serviceId);

            if (service is null)
            {
                throw new NotFoundException("Service not found");
            }

            var orderDetail = await _unitOfWork.GetOrderDetailRepository.FindOneAsync(od => od.OrderId == order.Id && od.ServiceId == service.Id);

            if (orderDetail is null)
            {

                orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ServiceId = service.Id,
                };
                double totalPrice = (double)order.TotalPrice;


                totalPrice -= service.PromotionId != null
                    ? (double)service.Price * (1 - (double)service.Promotion.DiscountPercent)
                    : (double)service.Price;


                order.TotalPrice = (decimal)totalPrice;
                await _unitOfWork.GetOrderRepository.UpdateAsync(order);
                await _unitOfWork.GetOrderDetailRepository.AddAsync(orderDetail);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new BadRequestException("Cart already has this service");
            }
        }

        public async Task<List<OrderDetailResponse>> GetList()
        {
            var orderDetails = await _unitOfWork.GetOrderDetailRepository.GetAllAsync();
            var orderDetailResponses = orderDetails.Adapt<List<OrderDetailResponse>>();
            foreach (var orderDetailResponse in orderDetailResponses)
            {
                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync( orderDetailResponse.ServiceId);
                orderDetailResponse.Subtotal = service.PromotionId != null ? service.Price*(1 - service.Promotion.DiscountPercent) : service.Price;
            }
            return orderDetailResponses;
        }

        public async Task RemoveFromCart(string userId, string serviceId)
        {
            var order = await _unitOfWork.GetOrderRepository.FindOneAsync(o => o.AccountId == userId && o.Status == OrderStatus.Incart.ToString());

            var orderDetail = await _unitOfWork.GetOrderDetailRepository.FindOneAsync(od => od.OrderId == order.Id && od.ServiceId == serviceId);

            if (orderDetail is null)
            {
                throw new BadRequestException("Cart not has this service");
            }
            else
            {
                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(serviceId);
                double totalPrice = (double)order.TotalPrice;

             
                totalPrice -= service.PromotionId != null
                    ? (double)service.Price * (1 - (double)service.Promotion.DiscountPercent)
                    : (double)service.Price;

                
                order.TotalPrice = (decimal)totalPrice;
                await _unitOfWork.GetOrderRepository.UpdateAsync(order);
                await _unitOfWork.GetOrderDetailRepository.DeleteAsync(orderDetail);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
