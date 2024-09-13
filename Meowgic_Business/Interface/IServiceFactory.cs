using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IServiceFactory
    {
        IAccountService GetAccountService();
        IAuthService GetAuthService();
        ICardService GetCardService();
        ICardMeaningService GetCardMeaningService();
        ICategoryService GetCategoryService();
        IOrderDetailService GetOrderDetailService();
        IOrderService GetOrderService();
        IPromotionService GetPromotionService();
        IQuestionService GetQuestionService();
        IServiceService GetServiceService();
        IFirebaseStorageService GetFirebaseStorageService();
        ITokenService GetTokenService();

    }
}
