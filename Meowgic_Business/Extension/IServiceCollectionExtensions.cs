using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Business.Services;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Promotion;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Request.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Extension
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicDependencies(this IServiceCollection services)
        {
            services.AddHostedService<BackgroundWorkerService>();
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICardMeaningService, CardMeaningService>();
            //services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IServiceService, TarotServiceService>();
            services.AddScoped<IZodiacService, ZodiacService>();
            services.AddScoped<IZodiacColorService, ZodiacColorService>();  
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IEmailService, EmailService>();  
            services.AddScoped<IScheduleReaderService, ScheduleReaderService>();
            services.AddScoped<IPayOSService, PayOSService>();
            return services;
        }
    }
}
