using Meowgic.Business.Interface;
using Meowgic.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<ITokenService> _tokenService;
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<ICardMeaningService> _cardMeaningService;
        private readonly Lazy<ICardService> _cardService;
        private readonly Lazy<ICategoryService> _categoryService;
        //private readonly Lazy<IFirebaseStorageService> _firebaseStorageService;
        private readonly Lazy<IOrderDetailService> _orderDetailService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IPromotionService> _promotionService;
        private readonly Lazy<IQuestionService> _questionService;
        private readonly Lazy<IServiceService> _serviceService;
        private readonly Lazy<IPayOSService> _payOSService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ServiceFactory(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _accountService = new Lazy<IAccountService>(() => new AccountService(unitOfWork));
            _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));
            _authService = new Lazy<IAuthService>(() => new AuthService(unitOfWork, this));
            //_cardMeaningService = new Lazy<ICardMeaningService>(() => new CardMeaningService(unitOfWork));
            //_cardService = new Lazy<ICardService>(() => new CardService(unitOfWork));
            //_categoryService = new Lazy<ICategoryService>(() => new CategoryService(unitOfWork));
            //_firebaseStorageService = new Lazy<IFirebaseStorageService>(() => new FirebaseStorageService(configuration));
            _orderDetailService = new Lazy<IOrderDetailService>(() => new OrderDetailService(unitOfWork));
            _orderService = new Lazy<IOrderService>(() => new OrderService(unitOfWork));
            _promotionService = new Lazy<IPromotionService>(() => new PromotionService(unitOfWork));
            _questionService = new Lazy<IQuestionService>(() => new QuestionService(unitOfWork));
            _payOSService = new Lazy<IPayOSService>(() => new PayOSService(configuration, unitOfWork));
            //_serviceService = new Lazy<IServiceService>(() => new ServiceService(unitOfWork));
        }

        public IAuthService GetAuthService => _authService.Value;

        public IAccountService GetAccountService => _accountService.Value;

        public ITokenService GetTokenService => _tokenService.Value;
        //public ICardMeaningService GetCardMeaningService => _cardMeaningService.Value;
        //public ICardService GetCardService => _cardService.Value;
        //public ICategoryService GetCategoryService => _categoryService.Value;
        //public IFirebaseStorageService GetFirebaseStorageService => _firebaseStorageService.Value;
        public IOrderDetailService GetOrderDetailService => _orderDetailService.Value;
        public IOrderService GetOrderService => _orderService.Value;
        public IPromotionService GetPromotionService => _promotionService.Value;
        public IQuestionService GetQuestionService => _questionService.Value;
        //public IServiceService GetServiceService => _serviceService.Value;
        public IPayOSService GetPayOSService => _payOSService.Value;
    }
}
