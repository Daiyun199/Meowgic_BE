using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IAccountRepository GetAccountRepository();
        ICardMeaningRepository GetCardMeaningRepository();
        ICardRepository GetCardRepository();
        ICategoryRepository GetCategoryRepository();
        IOrderDetailRepository GetOrderDetailRepository();
        IOrderRepository GetOrderRepository();
        IPromotionRepository GetPromotionRepository();
        IQuestionRepository GetQuestionRepository();
        IServiceRepository GetServiceRepository();
    }
}
