using Meowgic.Data.Models.Response.PayOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IPayOSService
    {
        Task<CreatePaymentResult> CreatePaymentLink(string orderId);
        Task<PaymentLinkInformation> GetPaymentLinkInformation(int orderCode);
        Task<PaymentLinkInformation> CancelOrder(int orderCode);
        Task<ResultModel> VerifyPaymentWebhookData(WebhookType body);
    }
}
