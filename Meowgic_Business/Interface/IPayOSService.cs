using Meowgic.Data.Models.Response.PayOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IPayOSService
    {
        Task<CreatePaymentResult> CreatePaymentLink(string orderId, ClaimsPrincipal claim);
        //Task<CreatePaymentResult> CreatePaymentLink(string name, decimal price);
        Task<PaymentLinkInformation> GetPaymentLinkInformation(int orderCode);
        Task<PaymentLinkInformation> CancelOrder(int orderCode);
        Task<ResultModel> VerifyPaymentWebhookData(WebhookType body);
    }
}
