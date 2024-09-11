using Google.Cloud.Storage.V1;
using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Business.Services;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Promotion;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Request.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Extension
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapsterConfigurations();
            services.AddSingleton(opt => StorageClient.Create());
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICardMeaningService, CardMeaningService>();
            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IServiceService, ServiceService>();
            return services;
        }

        private static void AddMapsterConfigurations(this IServiceCollection services)
        {
            TypeAdapterConfig<Register, Account>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<CardRequest, Card>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<CardMeaningRequest, CardMeaning>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<CategoryRequest, Category>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<CreatePromotion, Promotion>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<QuestionRequest, Question>.NewConfig().IgnoreNullValues(true);
            TypeAdapterConfig<ServiceRequest, TarotService>.NewConfig().IgnoreNullValues(true);
        }
    }
}
