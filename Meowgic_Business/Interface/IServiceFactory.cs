using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IServiceFactory
    {
        IAccountService GetAccountService { get; }
        IAuthService GetAuthService { get; }
        //ICardService GetCardService { get; }
        //ICardMeaningService GetCardMeaningService { get; }
        //ICategoryService GetCategoryService { get; }
        IOrderDetailService GetOrderDetailService { get; }
        IOrderService GetOrderService { get; }
        IPromotionService GetPromotionService { get; }
        IQuestionService GetQuestionService { get; }
        //IServiceService GetServiceService { get; }
        //IFirebaseStorageService GetFirebaseStorageService();
        ITokenService GetTokenService { get; }
        IPayOSService GetPayOSService { get; }
  

    }
}
