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
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using Meowgic.Shares.Exceptions;

namespace Meowgic.Business.Services
{
    public class PayOSService(IConfiguration configuration, IUnitOfWork unitOfWork) : IPayOSService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CreatePaymentResult> CreatePaymentLink(string orderId, ClaimsPrincipal claim)
        //public async Task<CreatePaymentResult> CreatePaymentLink(string name, decimal price)
        {
            PayOS _payOS = new(
                _configuration["payOS:ClientId"] ?? throw new Exception("Cannot find client"),
                _configuration["payOS:ApiKey"] ?? throw new Exception("Cannot find api key"),
                _configuration["payOS:ChecksumKey"] ?? throw new Exception("Cannot find Checksum Key")
            );

            //int orderCode = int.Parse(DateTime.Now.ToString("fffff"));
            //ItemData item = new(name, 1, (int)price );
            //List<ItemData> items = [item];
            //int expiredAt =  (int) (DateTime.UtcNow.AddMinutes(15) - DateTime.Now).TotalSeconds;

            //PaymentData paymentData = new(
            //    orderCode: orderCode,
            //    amount: (int)price,
            //    description: name,
            //    items: items,
            //    cancelUrl: "https://www.youtube.com/watch?v=XLr8rWFduUU",
            //    returnUrl: "https://www.facebook.com/",
            //    buyerName: "abc",
            //    buyerEmail: "abc",
            //    buyerPhone: "abc",
            //    expiredAt: expiredAt
            //    );
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);
            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            int orderCode = int.Parse(orderId[2..]);
            var order = await _unitOfWork.GetOrderRepository.GetByIdAsync(orderId);
            var orderDetails = order.OrderDetails;
            List<ItemData> items = [];
            foreach (var orderDetail in orderDetails)
            {
                var price = orderDetail.Service.PromotionId == null ? (int)orderDetail.Service.Price : (int)orderDetail.Service.Price * (int)orderDetail.Service.Promotion.DiscountPercent;
                items.Add(new ItemData(orderDetail.Service.Name, 1, price));
            }
            long expiredAt = (long)(DateTime.UtcNow.AddMinutes(15) - new DateTime(1970, 1, 1)).TotalSeconds;
            string signature = "amount="+ (int)order.TotalPrice 
                             + "buyerEmail=" + account.Email
                             + "buyerName=" + account.Name
                             + "buyerPhone=" + account.Phone
                             + "cancelUrl=https://www.youtube.com/watch?v=XLr8rWFduUU"
                             + "description=" + orderId
                             + "expiredAt=" + expiredAt
                             + "items=" + items
                             + "orderCode=" + orderCode
                             + "returnUrl=https://www.facebook.com/";

            PaymentData paymentData = new(
                orderCode: orderCode,
                amount: (int)order.TotalPrice,
                description: orderId,
                items: items,
                cancelUrl: "https://www.youtube.com/watch?v=XLr8rWFduUU",
                returnUrl: "https://www.facebook.com/",
                expiredAt: expiredAt,
                buyerName: account.Name,
                buyerPhone: account.Phone,
                buyerEmail: account.Email,
                signature: signature
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

            var order = await _unitOfWork.GetOrderRepository.GetByIdAsync("OD" + orderCode.ToString("D4"));
            order.Status = OrderStatus.Cancel.ToString();
            await _unitOfWork.GetOrderRepository.UpdateAsync(order);

            return paymentLinkInformation;
        }
        public async Task<ResultModel> VerifyPaymentWebhookData(WebhookType body)
        {
            try
            {
                PayOS _payOS = new(
                    _configuration["payOS:ClientId"] ?? throw new Exception("Cannot find client"),
                    _configuration["payOS:ApiKey"] ?? throw new Exception("Cannot find api key"),
                    _configuration["payOS:ChecksumKey"] ?? throw new Exception("Cannot find Checksum Key")
                );
                WebhookData data = _payOS.verifyPaymentWebhookData(body);

                string responseCode = data.code;
                var order = await _unitOfWork.GetOrderRepository.GetByIdAsync("OD" + data.orderCode.ToString("D4"));

                if (order != null && responseCode == "00")
                {
                    order.Status = OrderStatus.Paid.ToString(); // Success
                    await _unitOfWork.GetOrderRepository.UpdateAsync(order);
                    return new ResultModel { IsSuccess = true, Code = int.Parse(data.orderCode.ToString("D4")), Message = "Payment success" };
                }
                else
                {
                    if (order != null)
                    {
                        order.Status = OrderStatus.Cancel.ToString(); // Faild
                        await _unitOfWork.GetOrderRepository.UpdateAsync(order);
                    }
                }
                return new ResultModel { IsSuccess = false, Code = int.Parse(data.orderCode.ToString("D4")), Message = "Payment failed" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResultModel { IsSuccess = false, Code = -1, Message = "Payment failed" };
            }
        }
    }
}
