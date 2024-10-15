using Meowgic.Business.Interface;
using Meowgic.Data;
using Meowgic.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using Net.payOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meowgic.Data.Entities;
using Meowgic.Shares.Enum;
using Meowgic.Data.Models.Response.PayOS;

namespace Meowgic.Business.Services
{
    public class PayOSService(IConfiguration configuration, IUnitOfWork unitOfWork) : IPayOSService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CreatePaymentResult> CreatePaymentLink(string orderId)
        {
            PayOS _payOS = new(
                _configuration["payOS:ClientId"] ?? throw new Exception("Cannot find client"),
                _configuration["payOS:ApiKey"] ?? throw new Exception("Cannot find api key"),
                _configuration["payOS:ChecksumKey"] ?? throw new Exception("Cannot find Checksum Key")
            );
            int orderCode = int.Parse(orderId[2..]);
            var order = await _unitOfWork.GetOrderRepository.GetByIdAsync(orderId);
            var orderDetails = order.OrderDetails;
            List<ItemData> items = [];
            foreach (var orderDetail in orderDetails)
            {
                var price = orderDetail.Service.PromotionId == null ? (int)orderDetail.Service.Price : (int)orderDetail.Service.Price * (int)orderDetail.Service.Promotion.DiscountPercent;
                items.Add(new ItemData(orderDetail.Service.Name, 1, price));
            }
            long expiredAt = (long)(DateTime.UtcNow.AddMinutes(10) - new DateTime(1970, 1, 1)).TotalSeconds;

            PaymentData paymentData = new(
                orderCode: orderCode,
                amount: (int)order.TotalPrice,
                description: order.Id,
                items: items,
                cancelUrl: "https://www.youtube.com/watch?v=XLr8rWFduUU",
                returnUrl: "https://www.facebook.com/",
                expiredAt: expiredAt
                );

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            return createPayment;
        }

        public async Task<PaymentLinkInformation> GetPaymentLinkInformation(int orderCode)
        {
            PayOS _payOS = new(
                _configuration["payOS:ClientId"] ?? throw new Exception("Cannot find client"),
                _configuration["payOS:ApiKey"] ?? throw new Exception("Cannot find api key"),
                _configuration["payOS:ChecksumKey"] ?? throw new Exception("Cannot find Checksum Key")
            );
            PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderCode);
            return paymentLinkInformation;
        }
        public async Task<PaymentLinkInformation> CancelOrder(int orderCode)
        {
            PayOS _payOS = new(
                    _configuration["payOS:ClientId"] ?? throw new Exception("Cannot find client"),
                    _configuration["payOS:ApiKey"] ?? throw new Exception("Cannot find api key"),
                    _configuration["payOS:ChecksumKey"] ?? throw new Exception("Cannot find Checksum Key")
                );
            PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderCode);

            var order = await _unitOfWork.GetOrderRepository.GetByIdAsync("OD" + orderCode.ToString());
            order.Status = OrderStatus.Cancel.ToString();
            await _unitOfWork.GetOrderRepository.UpdateAsync(order);

            return paymentLinkInformation;
        }
        public async Task<ResultModel> VerifyPaymentWebhookData(WebhookType body)
        {
            PayOS _payOS = new(
                    _configuration["payOS:ClientId"] ?? throw new Exception("Cannot find client"),
                    _configuration["payOS:ApiKey"] ?? throw new Exception("Cannot find api key"),
                    _configuration["payOS:ChecksumKey"] ?? throw new Exception("Cannot find Checksum Key")
                );
            WebhookData data = _payOS.verifyPaymentWebhookData(body);

            string responseCode = data.code;
            var order = await _unitOfWork.GetOrderRepository.GetByIdAsync("OD" + data.orderCode.ToString());

            if (order != null && responseCode == "00")
            {
                order.Status = OrderStatus.Paid.ToString(); // Success
                await _unitOfWork.GetOrderRepository.UpdateAsync(order);
                return new ResultModel{ IsSuccess = true, Code = int.Parse(data.orderCode.ToString()), Message = "Payment success" };
            }
            else
            {
                if (order != null)
                {
                    order.Status = OrderStatus.Cancel.ToString(); // Faild
                    await _unitOfWork.GetOrderRepository.UpdateAsync(order);
                }
            }
            return new ResultModel { IsSuccess = false, Code = int.Parse(data.orderCode.ToString()), Message = "Payment failed" };
        }
    }
}
