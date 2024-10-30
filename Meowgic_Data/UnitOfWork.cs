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
        private readonly Lazy<IFeedbackRepository> _feedbackRepository;
        private readonly Lazy<IOrderDetailRepository> _orderDetailRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<IPromotionRepository> _promotionRepository;
        private readonly Lazy<IQuestionRepository> _questionRepository;
        private readonly Lazy<IServiceRepository> _serviceRepository;
        private readonly Lazy<IScheduleReaderRepository> _scheduleReaderRepository;
        private readonly Lazy<IZodiacColorRepository> _colorRepository;
        private readonly Lazy<IZodiacRepository> _zodiacRepository;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _cardRepository = new Lazy<ICardRepository>(() => new CardRepository(context));
            _cardMeaningRepository = new Lazy<ICardMeaningRepository>(() => new CardMeaningRepository(context));
            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));
            _feedbackRepository = new Lazy<IFeedbackRepository>(() => new FeedbackRepository(context));
            _orderDetailRepository = new Lazy<IOrderDetailRepository>(() => new OrderDetailRepository(context));
            _orderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(context));
            _promotionRepository = new Lazy<IPromotionRepository>(() => new PromotionRepository(context));
            _questionRepository = new Lazy<IQuestionRepository>(() => new QuestionRepository(context));
            _serviceRepository = new Lazy<IServiceRepository>(() => new ServiceRepository(context));
            _scheduleReaderRepository = new Lazy<IScheduleReaderRepository>(() => new ScheduleReaderRepository(context));
            _zodiacRepository = new Lazy<IZodiacRepository>(() => new ZodiacRepository(context));
            _colorRepository = new Lazy<IZodiacColorRepository>(() => new ZodiacColorRepository(context));
        }

        public IAccountRepository GetAccountRepository => _accountRepository.Value;

        public ICardRepository GetCardRepository => _cardRepository.Value;

        public ICardMeaningRepository GetCardMeaningRepository => _cardMeaningRepository.Value;

        public ICategoryRepository GetCategoryRepository =>  _categoryRepository.Value;
        public IFeedbackRepository GetFeedbackRepository => _feedbackRepository.Value;

        public IOrderDetailRepository GetOrderDetailRepository => _orderDetailRepository.Value;
        public IOrderRepository GetOrderRepository => _orderRepository.Value;
        public IPromotionRepository GetPromotionRepository => _promotionRepository.Value;
        public IQuestionRepository GetQuestionRepository => _questionRepository.Value;
        public IServiceRepository GetServiceRepository => _serviceRepository.Value;
        public IScheduleReaderRepository GetScheduleReaderRepository => _scheduleReaderRepository.Value;
        public IZodiacColorRepository GetZodiacColorRepository => _colorRepository.Value;
        public IZodiacRepository GetZodiacRepository => _zodiacRepository.Value;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
