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

        IAccountRepository GetAccountRepository { get; }
        ICardMeaningRepository GetCardMeaningRepository { get; }
        ICardRepository GetCardRepository { get; }
        ICategoryRepository GetCategoryRepository {  get; }
        IOrderDetailRepository GetOrderDetailRepository { get; }
        IOrderRepository GetOrderRepository { get; }
        IPromotionRepository GetPromotionRepository { get; }
        IQuestionRepository GetQuestionRepository { get; }
        IServiceRepository GetServiceRepository { get; }
    }
}
