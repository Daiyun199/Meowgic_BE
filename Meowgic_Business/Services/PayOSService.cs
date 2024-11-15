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
using Org.BouncyCastle.Ocsp;

namespace Meowgic.Business.Services
{
    public class PayOSService(IConfiguration configuration, IUnitOfWork unitOfWork) : IPayOSService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultModel> CreatePaymentLink(string orderId, ClaimsPrincipal claim)
        //public async Task<CreatePaymentResult> CreatePaymentLink(string name, decimal price)
        {
            var clientId = _configuration["payOS:ClientId"];
            if (clientId == null)
            {
                return new ResultModel { IsSuccess = false, Message = "ClientId not found" };
            }
            var apiKey = _configuration["payOS:ApiKey"];
            if (apiKey == null)
            {
                return new ResultModel { IsSuccess = false, Message = "ApiKey not found" };
            }
            var checksumKey = _configuration["payOS:ChecksumKey"];
            if (checksumKey == null)
            {
                return new ResultModel { IsSuccess = false, Message = "ChecksumKey not found" };
            }

            PayOS _payOS = new(
                clientId,
                apiKey,
                checksumKey
            );

            
            
            //var paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderCode);
            //if (paymentLinkInformation is not null)
            //{
            //    return null;
            //}
            var order = await _unitOfWork.GetOrderRepository.GetOrderDetailsInfoById(orderId);
            if (order is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Order not found!!" };
            }
            var userId = claim.FindFirst("aid")?.Value;
            var account = await _unitOfWork.GetAccountRepository.GetByIdAsync(userId);
            if (account is null)
            {
                return new ResultModel { IsSuccess = false, Message = "Account not found!!" };
            }
            if (order.AccountId != account.Id)
            {
                return new ResultModel { IsSuccess = false, Message = "Account not have permission!!" };
            }

            //order.Status = OrderStatus.Paid.ToString(); // Success
            //await _unitOfWork.GetOrderRepository.UpdateAsync(order);
            //await _unitOfWork.SaveChangesAsync();

            int orderCode = int.Parse(orderId[2..]);

            var orderDetails = order.OrderDetails;
            List<ItemData> items = [];
            foreach (var orderDetail in orderDetails)
            {
                var service = await _unitOfWork.GetServiceRepository.GetTarotServiceByIdAsync(orderDetail.ServiceId);
                var promotion = await _unitOfWork.GetPromotionRepository.GetByIdAsync(service.PromotionId);
                var price = service.PromotionId == null ? (int)service.Price : (int)service.Price * (int)promotion.DiscountPercent;
                items.Add(new ItemData(service.Name, 1, price));
            }
            long expiredAt = (long)(DateTime.UtcNow.AddMinutes(30) - new DateTime(1970, 1, 1)).TotalSeconds;

            PaymentData paymentData = new(
                orderCode: orderCode,
                amount: (int)order.TotalPrice,
                description: "Thanh toan don " + orderId,
                items: items,
                cancelUrl: "https://www.meowgic.online/fail",
                returnUrl: "https://www.meowgic.online/success",
                expiredAt: expiredAt,
                buyerName: account.Name,
                buyerPhone: account.Phone,
                buyerEmail: account.Email
            );

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            return new ResultModel { IsSuccess = true, Message = "Create payment successfully!!", Data = createPayment};
        }

        public async Task<ResultModel> GetPaymentLinkInformation(long orderCode)
        {
            var clientId = _configuration["payOS:ClientId"];
            if (clientId == null)
            {
                return new ResultModel { IsSuccess = false, Message = "ClientId not found" };
            }
            var apiKey = _configuration["payOS:ApiKey"];
            if (apiKey == null)
            {
                return new ResultModel { IsSuccess = false, Message = "ApiKey not found" };
            }
            var checksumKey = _configuration["payOS:ChecksumKey"];
            if (checksumKey == null)
            {
                return new ResultModel { IsSuccess = false, Message = "ChecksumKey not found" };
            }

            PayOS _payOS = new(clientId,apiKey,checksumKey);
            PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderCode);
            return paymentLinkInformation == null ? 
                new ResultModel { IsSuccess = false, Message = "Payment not found!!" } 
                : new ResultModel { IsSuccess = true, Message = "Get payment successfully!!!!", Data = paymentLinkInformation }; ;
        }
        public async Task<ResultModel> CancelOrder(int orderCode)
        {
            PayOS _payOS = new(
                    _configuration["payOS:ClientId"] ?? throw new Exception("Cannot find client"),
                    _configuration["payOS:ApiKey"] ?? throw new Exception("Cannot find api key"),
                    _configuration["payOS:ChecksumKey"] ?? throw new Exception("Cannot find Checksum Key")
                );
            var getPaymentLinkInformation = await _payOS.getPaymentLinkInformation((long)orderCode);
            if (getPaymentLinkInformation == null)
            {
                return new ResultModel { IsSuccess = false, Message = "Payment not found!!" };
            }
            PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderCode);

            var order = await _unitOfWork.GetOrderRepository.GetByIdAsync("OD" + orderCode.ToString("D4"));
            order.Status = OrderStatus.Cancel.ToString();
            await _unitOfWork.GetOrderRepository.UpdateAsync(order);

            return new ResultModel { IsSuccess = true, Message = "Canceled payment successfully!!" , Data = paymentLinkInformation};
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
                string orderCode = data.orderCode.ToString("D4");
                var order = await _unitOfWork.GetOrderRepository.GetByIdAsync("OD" + orderCode);

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
