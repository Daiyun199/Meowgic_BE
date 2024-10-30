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
using Meowgic.Data.Models.Request.OrderDetail;
using Org.BouncyCastle.Asn1.Ocsp;
using Meowgic.Data.Models.Response.Order;

namespace Meowgic.Business.Services
{
    public class OrderDetailService(IUnitOfWork unitOfWork) : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<OrderDetailResponse> AddToCart(ClaimsPrincipal claim, AddToCartRequest request)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }

            var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(request.ServiceId);

            if (service is null)
            {
                throw new NotFoundException("Service not found");
            }
            var availableSchedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(request.ScheduleReaderId);

            if (availableSchedule.IsBooked)
            {
                throw new BadRequestException("This schedule not availabe");
            }

            var existingService = await _unitOfWork.GetOrderDetailRepository.FindOneAsync(od => od.ServiceId == request.ServiceId && od.OrderId == null);

            if (existingService is null)
            {
                var orderDetail = request.Adapt<OrderDetail>();
                orderDetail.CreatedBy = userId;
                orderDetail.CreatedTime = DateTime.Now;
                await _unitOfWork.GetOrderDetailRepository.AddAsync(orderDetail);
                await _unitOfWork.SaveChangesAsync();
                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);

                var result = new OrderDetailResponse
                {
                    Id = orderDetail.Id,
                    ServiceName = service.Name,
                    OrderId = orderDetail.OrderId,
                    Date = schedule.DayOfWeek.ToString("dd/MM/yyyy"),
                    StartTime = schedule.StartTime.ToString("hh:mm:ss tt"),
                    EndTime = schedule.EndTime.ToString("hh:mm:ss tt"),
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price
                };
                return result;
            }
            else
            {
                throw new BadRequestException("Cart already has this service");
            }
        }

        public async Task<List<OrderDetailResponse>> GetCart(ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);
            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }

            var orderDetails = await _unitOfWork.GetOrderDetailRepository.GetCart(userId);

            var result = new List<OrderDetailResponse>();
            foreach ( var orderDetail in orderDetails)
            {
                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(orderDetail.ServiceId);
                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);
                var orderDetailResponse = new OrderDetailResponse
                {
                    Id = orderDetail.Id,
                    ServiceName = service.Name,
                    OrderId = orderDetail.OrderId,
                    Date = schedule.DayOfWeek.ToString("dd/MM/yyyy"),
                    StartTime = schedule.StartTime.ToString("hh:mm:ss tt"),
                    EndTime = schedule.EndTime.ToString("hh:mm:ss tt"),
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price
                };
                result.Add(orderDetailResponse);
            }
            return result;
        }

        public async Task<OrderDetailResponse> RemoveFromCart(ClaimsPrincipal claim, string detailId)
        {
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);
            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }

            var orderDetail = await _unitOfWork.GetOrderDetailRepository.FindOneAsync(od => od.Id == detailId);

            if (orderDetail is null)
            {
                throw new BadRequestException("Cart not has this service");
            }
            else
            {
                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(orderDetail.ServiceId);
                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);
                var result = new OrderDetailResponse
                {
                    Id = orderDetail.Id,
                    ServiceName = service.Name,
                    OrderId = orderDetail.OrderId,
                    Date = schedule.DayOfWeek.ToString("dd/MM/yyyy"),
                    StartTime = schedule.StartTime.ToString("hh:mm:ss tt"),
                    EndTime = schedule.EndTime.ToString("hh:mm:ss tt"),
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price
                };
                await _unitOfWork.GetOrderDetailRepository.DeleteAsync(orderDetail);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
        }
        public async Task<OrderDetailResponse> UpdateOrderDetail(ClaimsPrincipal claim, string detailId, UpdateDetailInfor request)
        {
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);
            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }

            var orderDetail = await _unitOfWork.GetOrderDetailRepository.FindOneAsync(od => od.Id == detailId);

            if (orderDetail is null)
            {
                throw new BadRequestException("Cart not has this service");
            }
            else
            {
                orderDetail.ScheduleReaderId = request.ScheduleReaderId;
                await _unitOfWork.GetOrderDetailRepository.UpdateAsync(orderDetail);
                await _unitOfWork.SaveChangesAsync();

                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(orderDetail.ServiceId);
                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);
                var result = new OrderDetailResponse
                {
                    Id = orderDetail.Id,
                    ServiceName = service.Name,
                    OrderId = orderDetail.OrderId,
                    Date = schedule.DayOfWeek.ToString("dd/MM/yyyy"),
                    StartTime = schedule.StartTime.ToString("hh:mm:ss tt"),
                    EndTime = schedule.EndTime.ToString("hh:mm:ss tt"),
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price
                };
                return result;
            }
        }
        public async Task<OrderDetail> GetOrderDetailById(string id)
        {
            var orderDetail = await _unitOfWork.GetOrderDetailRepository.GetOrderDetailByIdAsync(id);

            if (orderDetail is null)
            {
                throw new BadRequestException("Not found!!!");
            }
                return orderDetail;
        }

        public async Task<List<OrderDetailResponse>> GetAll()
        {
            var orderDetails = await _unitOfWork.GetOrderDetailRepository.GetAllOrderDetails();

            var result = new List<OrderDetailResponse>();
            foreach (var orderDetail in orderDetails)
            {
                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(orderDetail.ServiceId);
                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);
                var orderDetailResponse = new OrderDetailResponse
                {
                    Id = orderDetail.Id,
                    ServiceName = service.Name,
                    OrderId = orderDetail.OrderId,
                    Date = schedule.DayOfWeek.ToString("dd/MM/yyyy"),
                    StartTime = schedule.StartTime.ToString("hh:mm:ss tt"),
                    EndTime = schedule.EndTime.ToString("hh:mm:ss tt"),
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price
                };
                result.Add(orderDetailResponse);
            }
            return result;
        }
        public async Task<List<OrderDetailResponse>> GetAllByOrderId(string orderId)
        {
            var orderDetails = await _unitOfWork.GetOrderDetailRepository.GetAllOrderDetailsByOrderId(orderId);

            var result = new List<OrderDetailResponse>();
            foreach (var orderDetail in orderDetails)
            {
                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(orderDetail.ServiceId);
                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);
                var orderDetailResponse = new OrderDetailResponse
                {
                    Id = orderDetail.Id,
                    ServiceName = service.Name,
                    OrderId = orderDetail.OrderId,
                    Date = schedule.DayOfWeek.ToString("dd/MM/yyyy"),
                    StartTime = schedule.StartTime.ToString("hh:mm:ss tt"),
                    EndTime = schedule.EndTime.ToString("hh:mm:ss tt"),
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price
                };
                result.Add(orderDetailResponse);
            }
            return result;
        }
    }
}
