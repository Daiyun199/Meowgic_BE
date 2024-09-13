using Meowgic.Business.Interface;
using Meowgic.Business.Services;
using Meowgic.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<ITokenService> _tokenService;
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<ICardMeaningService> _cardMeaningService;
        private readonly Lazy<ICardService> _cardService;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IFirebaseStorageService> _firebaseStorageService;
        private readonly Lazy<IOrderDetailService> _orderDetailService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IPromotionService> _promotionService;
        private readonly Lazy<IQuestionService> _questionService;
        private readonly Lazy<IServiceService> _serviceService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ServiceFactory(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _accountService = new Lazy<IAccountService>(() => new AccountService(unitOfWork));
            _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));
            _authService = new Lazy<IAuthService>(() => new AuthService(unitOfWork, this));
            _cardMeaningService = new Lazy<ICardMeaningService>(() => new CardMeaningService(unitOfWork));
            _cardService = new Lazy<ICardService>(() => new CardService(unitOfWork));
            _categoryService = new Lazy<ICategoryService>(() => new CategoryService(unitOfWork));
            _firebaseStorageService = new Lazy<IFirebaseStorageService>(() => new FirebaseStorageService(configuration));
            _orderDetailService = new Lazy<IOrderDetailService>(() => new OrderDetailService(unitOfWork));
            _orderService = new Lazy<IOrderService>(() => new OrderService(unitOfWork));
            _promotionService = new Lazy<IPromotionService>(() => new PromotionService(unitOfWork));
            _questionService = new Lazy<IQuestionService>(() => new QuestionService(unitOfWork));
            _serviceService = new Lazy<IServiceService>(() => new ServiceService(unitOfWork));
        }

        public IAuthService GetAuthService()
        {
            return _authService.Value;
        }

        public IAccountService GetAccountService()
        {
            return _accountService.Value;
        }

        public ITokenService GetTokenService()
        {
            return _tokenService.Value;
        }
        public ICardMeaningService GetCardMeaningService()
        {
            return _cardMeaningService.Value;
        }
        public ICardService GetCardService()
        {
            return _cardService.Value;
        }
        public ICategoryService GetCategoryService()
        {
            return _categoryService.Value;
        }
        public IFirebaseStorageService GetFirebaseStorageService()
        {
            return _firebaseStorageService.Value;
        }
        public IOrderDetailService GetOrderDetailService()
        {
            return _orderDetailService.Value;
        }
        public IOrderService GetOrderService()
        {
            return _orderService.Value;
        }
        public IPromotionService GetPromotionService()
        {
            return _promotionService.Value;
        }
        public IQuestionService GetQuestionService()
        {
            return _questionService.Value;
        }
        public IServiceService GetServiceService()
        {
            return _serviceService.Value;
        }
    }
}
