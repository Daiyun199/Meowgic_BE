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
using Meowgic.Data.Models.Response.PayOS;
using Microsoft.Identity.Client;

namespace Meowgic.Business.Services
{
    public class OrderDetailService(IUnitOfWork unitOfWork) : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultModel> AddToCart(ClaimsPrincipal claim, AddToCartRequest request)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Account not found!!" };
            }

            var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(request.ServiceId);

            if (service is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Service not found!!" };
            }
            var availableSchedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(request.ScheduleReaderId);
            if (availableSchedule is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Schedule not found!!" };
            }
            if (availableSchedule.IsBooked)
            {
                return new ResultModel { IsSuccess = false, Message = "This schedule is not available!!" };
            }

            var existingService = await _unitOfWork.GetOrderDetailRepository.FindOneAsync(od => 
                    od.ServiceId == request.ServiceId 
                    && od.OrderId == null 
                    && od.CreatedBy == userId 
                    && od.DeletedTime == null);

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
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price,
                    CreateBy = userId,
                };
                return new ResultModel { IsSuccess = true, Message = "Add to card success!!", Data = result }; ;
            }
            else
            {
                return new ResultModel { IsSuccess = false, Message = "Cart already has this service" };
            }
        }

        public async Task<ResultModel> GetCart(ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);
            if (account is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Account not found!!" };
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
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price,
                    CreateBy = userId
                };
                result.Add(orderDetailResponse);
            }
            return new ResultModel { IsSuccess = true, Message = "Get cart successfully!!", Data = result }; ;
        }

        public async Task<ResultModel> RemoveFromCart(ClaimsPrincipal claim, string detailId)
        {
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);
            if (account is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Account not found" };
            }

            var orderDetail = await _unitOfWork.GetOrderDetailRepository.FindOneAsync(od => od.Id == detailId);

            if (orderDetail is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Cart not has this service" };
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
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price,
                    CreateBy = orderDetail.CreatedBy
                };
                await _unitOfWork.GetOrderDetailRepository.DeleteAsync(orderDetail);
                await _unitOfWork.SaveChangesAsync();
                return new ResultModel { IsSuccess = true, Message = "Remove successfully!!", Data = result };
            }
        }
        public async Task<ResultModel> UpdateOrderDetail(ClaimsPrincipal claim, string detailId, UpdateDetailInfor request)
        {
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);
            if (account is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Account not found" };
            }

            var orderDetail = await _unitOfWork.GetOrderDetailRepository.FindOneAsync(od => od.Id == detailId);

            if (orderDetail is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Cart not has this service" };
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
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price,
                    CreateBy = orderDetail.CreatedBy
                };
                return new ResultModel { IsSuccess = true, Message = "Update successfully!!", Data = result };
            }
        }
        public async Task<ResultModel> GetOrderDetailById(string id)
        {
            var orderDetail = await _unitOfWork.GetOrderDetailRepository.GetOrderDetailByIdAsync(id);

            if (orderDetail is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Not found!!" };
            }
            return new ResultModel { IsSuccess = true, Message = "Get successfully!", Data = orderDetail };
        }

        public async Task<ResultModel> GetAll()
        {
            var orderDetails = await _unitOfWork.GetOrderDetailRepository.GetAllOrderDetails();

            var result = new List<OrderDetailResponse>();

            foreach (var orderDetail in orderDetails)
            {
                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(orderDetail.ServiceId);
                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);
                var order = await _unitOfWork.GetOrderRepository.GetOrderDetailsInfoById(orderDetail.OrderId);
                var orderDetailResponse = new OrderDetailResponse
                {
                    Id = orderDetail.Id,
                    ServiceName = service.Name,
                    OrderId = orderDetail.OrderId,
                    Date = schedule.DayOfWeek.ToString("dd/MM/yyyy"),
                    StartTime = schedule.StartTime.ToString("hh:mm:ss tt"),
                    EndTime = schedule.EndTime.ToString("hh:mm:ss tt"),
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price,
                    CreateBy = orderDetail.CreatedBy,
                    Status = order == null ? "Incart" : order.Status
                };
                result.Add(orderDetailResponse);
            }
            return new ResultModel { IsSuccess = true, Message = "Successfully!!", Data = result };
        }
        public async Task<ResultModel> GetAllByOrderId(string orderId)
        {
            var orderDetails = await _unitOfWork.GetOrderDetailRepository.GetAllOrderDetailsByOrderId(orderId);

            var result = new List<OrderDetailResponse>();
            foreach (var orderDetail in orderDetails)
            {
                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(orderDetail.ServiceId);
                var schedule = await _unitOfWork.GetScheduleReaderRepository.GetByIdAsync(orderDetail.ScheduleReaderId);
                var order = await _unitOfWork.GetOrderRepository.GetOrderDetailsInfoById(orderId);
                var orderDetailResponse = new OrderDetailResponse
                {
                    Id = orderDetail.Id,
                    ServiceName = service.Name,
                    OrderId = orderDetail.OrderId,
                    Date = schedule.DayOfWeek.ToString("dd/MM/yyyy"),
                    StartTime = schedule.StartTime.ToString("hh:mm:ss tt"),
                    EndTime = schedule.EndTime.ToString("hh:mm:ss tt"),
                    Subtotal = service.PromotionId != null ? (decimal)(service.Price * (1 - service.Promotion.DiscountPercent)) : (decimal)service.Price,
                    CreateBy = orderDetail.CreatedBy,
                    Status = order.Status
                };
                result.Add(orderDetailResponse);
            }
            return new ResultModel { IsSuccess = true, Message = "Successfully!!" , Data = result};
        }
    }
}
