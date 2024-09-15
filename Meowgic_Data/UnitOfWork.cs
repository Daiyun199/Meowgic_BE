using Meowgic.Data.Data;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private readonly Lazy<IAccountRepository> _accountRepository;
        private readonly Lazy<ICardRepository> _cardRepository;
        private readonly Lazy<ICardMeaningRepository> _cardMeaningRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<IOrderDetailRepository> _orderDetailRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<IPromotionRepository> _promotionRepository;
        private readonly Lazy<IQuestionRepository> _questionRepository;
        private readonly Lazy<IServiceRepository> _serviceRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _cardRepository = new Lazy<ICardRepository>(() => new CardRepository(context));
            _cardMeaningRepository = new Lazy<ICardMeaningRepository>(() => new CardMeaningRepository(context));
            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));
            _orderDetailRepository = new Lazy<IOrderDetailRepository>(() => new OrderDetailRepository(context));
            _orderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(context));
            _promotionRepository = new Lazy<IPromotionRepository>(() => new PromotionRepository(context));
            _questionRepository = new Lazy<IQuestionRepository>(() => new QuestionRepository(context));
            _serviceRepository = new Lazy<IServiceRepository>(() => new ServiceRepository(context));
        }

        public IAccountRepository GetAccountRepository()
        {
            return _accountRepository.Value;
        }

        public ICardRepository GetCardRepository()
        {
            return _cardRepository.Value;
        }

        public ICardMeaningRepository GetCardMeaningRepository()
        {
            return _cardMeaningRepository.Value;
        }

        public ICategoryRepository GetCategoryRepository()
        {
            return _categoryRepository.Value;
        }

        public IOrderDetailRepository GetOrderDetailRepository()
        {
            return _orderDetailRepository.Value;
        }
        public IOrderRepository GetOrderRepository()
        {
            return _orderRepository.Value;
        }
        public IPromotionRepository GetPromotionRepository()
        {
            return _promotionRepository.Value;
        }
        public IQuestionRepository GetQuestionRepository()
        {
            return _questionRepository.Value;
        }
        public IServiceRepository GetServiceRepository()
        {
            return _serviceRepository.Value;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
