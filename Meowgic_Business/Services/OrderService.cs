using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Order;
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
            return (await _unitOfWork.GetOrderRepository().GetPagedOrders(request)).Adapt<PagedResultResponse<OrderResponses>>();
        }

        public async Task<Order> GetOrderDetailsInfoById(string orderId)
        {
            var order = await _unitOfWork.GetOrderRepository().GetOrderDetailsInfoById(orderId);

            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            return order;
        }
        public async Task<Order> GetCartInfo(string userId)
        {
            var order = await _unitOfWork.GetOrderRepository().GetCustomerCartInfo(userId);
            if (order is null)
            {
                throw new NotFoundException("Empty cart");
            }
            return order;
        }
        public async Task ConfirmOrder(string userId, string orderId, List<string> serviceIds)
        {
            var order = await _unitOfWork.GetOrderRepository().FindOneAsync(o => o.Id == orderId);
            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }
            order.TotalPrice = 0;

            if (order.CreatedBy != userId)
            {
                throw new BadRequestException("The order does not belong to the customer.");
            }

            if (order.Status != "InCart")
            {
                throw new BadRequestException("The order status must be 'InCart'.");
            }

            var orderDetails = await _unitOfWork.GetOrderDetailRepository().FindAsync(o => o.OrderId == orderId);
            if (orderDetails is [])
            {
                throw new NotFoundException("There are no items in cart.");
            }

            if (serviceIds is null || serviceIds is [])
            {
                throw new BadRequestException("There are no product confirmed for order.");
            }

            order.Status = OrderStatus.Unpaid.ToString();

            foreach (var serviceId in serviceIds)
            {
                var orderDetail = await _unitOfWork.GetOrderDetailRepository().FindOneAsync(o => o.OrderId == orderId && o.ServiceId == serviceId);
                var serviceDetail = await _unitOfWork.GetServiceRepository().GetService(serviceId);

                if (orderDetail is null)
                {
                    throw new BadRequestException("The service is not in cart.");
                }
                if (serviceDetail is null)
                {
                    throw new NotFoundException("Service not found.");
                }
                orderDetails.Remove(orderDetails.First(o => o.OrderId == orderDetail.OrderId && o.ServiceId == orderDetail.ServiceId));
                order.TotalPrice += orderDetail.Service.PromotionId != null ? orderDetail.Service.Price*(1 - orderDetail.Service.Promotion.DiscountPercent) : orderDetail.Service.Price;
            }
            await _unitOfWork.SaveChangesAsync();

            if (orderDetails is not [])
            {
                await _unitOfWork.GetOrderRepository().AddAsync(new Order
                {
                    AccountId = userId,
                    Status = OrderStatus.Incart.ToString(),
                    TotalPrice = 0,
                    OrderDate = DateTime.Now
                });
                await _unitOfWork.SaveChangesAsync();

                foreach (var item in orderDetails)
                {
                    await _unitOfWork.GetOrderDetailRepository().DeleteAsync(item);
                    await _unitOfWork.GetOrderDetailRepository().AddAsync(new OrderDetail
                    {
                        OrderId = order.Id,
                        ServiceId = item.ServiceId
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task CancelOrder(string userId, string orderId)
        {
            var order = await _unitOfWork.GetOrderRepository().FindOneAsync(o => o.Id == orderId);
            var account = await _unitOfWork.GetAccountRepository().FindOneAsync(a => a.Id == userId);
            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            if (order.AccountId != userId && account.Role != "Staff")
            {
                throw new BadRequestException("The order does not belong to this account.");
            }

            if (order.Status != OrderStatus.Unpaid.ToString())
            {
                throw new BadRequestException("The order status must be 'Unpaid' to cancel.");
            }

            order.Status = OrderStatus.Cancel.ToString();

            var orderDetails = await _unitOfWork.GetOrderDetailRepository().FindAsync(o => o.OrderId == order.Id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateOrderDetail(string userId, string orderId, string serviceId)
        {
            var order = await _unitOfWork.GetOrderRepository().FindOneAsync(o => o.Id == orderId);
            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            if (order.Status != OrderStatus.Incart.ToString())
            {
                throw new BadRequestException("The order status must be 'InCart'.");
            }

            var orderDetail = await _unitOfWork.GetOrderDetailRepository().FindOneAsync(od => od.OrderId == orderId && od.ServiceId == serviceId);
            if (orderDetail is null)
            {
                throw new NotFoundException("Service not found in the order.");
            }

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteServiceFromCart(string userId, string orderId, string serviceId)
        {
            var order = await _unitOfWork.GetOrderRepository().FindOneAsync(o => o.Id == orderId);
            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            if (order.Status != OrderStatus.Incart.ToString())
            {
                throw new BadRequestException("The order status must be 'InCart'.");
            }

            var orderDetail = await _unitOfWork.GetOrderDetailRepository().FindOneAsync(od => od.OrderId == orderId && od.ServiceId == serviceId);
            if (orderDetail is null)
            {
                throw new NotFoundException("Shirt not found in the order.");
            }

            await _unitOfWork.GetOrderDetailRepository().DeleteAsync(orderDetail);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteOrder(string userId, string orderId)
        {    
            var order = await _unitOfWork.GetOrderRepository().FindOneAsync(o => o.Id == orderId);
            var account = await _unitOfWork.GetAccountRepository().FindOneAsync(a => a.Id == userId);
            if (order is null)
            {
                throw new NotFoundException("Order not found");
            }

            if (account.Role.Equals("Customer"))
            {
                if (order.AccountId != account.Id)
                {
                    throw new BadRequestException("The order does not belong to this account.");
                }
            }

            var orderDetails = await _unitOfWork.GetOrderDetailRepository().FindAsync(o => o.OrderId == order.Id);
            if (orderDetails is not [])
            {
                foreach (var item in orderDetails)
                {
                    await _unitOfWork.GetOrderDetailRepository().DeleteAsync(item);
                }
            }
            await _unitOfWork.GetOrderRepository().DeleteAsync(order);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
