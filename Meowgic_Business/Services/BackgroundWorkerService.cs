using Meowgic.Business.Interface;
using Meowgic.Data;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Promotion;
using Meowgic.Data.Models.Request.Service;
using Meowgic.Shares.Enum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class BackgroundWorkerService(ILogger<BackgroundWorkerService> logger, IServiceProvider service) : BackgroundService
    {
        readonly ILogger<BackgroundWorkerService> _logger = logger;
        private readonly IServiceProvider _service = service;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {                
                using (var scope = _service.CreateScope())
                {
                    var promotionService = scope.ServiceProvider.GetRequiredService<IPromotionService>();
                    var tarotService = scope.ServiceProvider.GetRequiredService<IServiceService>();

                    var promotions = await promotionService.GetAll();
                    foreach (var promotion in promotions)
                    {
                        if (promotion.ExpireTime.CompareTo(DateTime.Now) < 1)
                        {
                            await promotionService.DeletePromotion(promotion.Id, null);
                        }
                    }
                }
                _logger.LogInformation("Time: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }

        }
    }
}
