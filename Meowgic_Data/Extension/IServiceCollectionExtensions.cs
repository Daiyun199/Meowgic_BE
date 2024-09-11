using Meowgic.Data.Interfaces;
using Meowgic.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Extension
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoriesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICardMeaningRepository, CardMeaningRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            return services;
        }
    }
}
