﻿using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IServiceService
    {
        Task<TarotService> CreateTarotServiceAsync(ServiceRequest tarotServiceRequest);
        Task<TarotService?> GetTarotServiceByIdAsync(string id);
        Task<IEnumerable<TarotService>> GetAllTarotServicesAsync();
        Task<TarotService?> UpdateTarotServiceAsync(string id, ServiceRequest tarotServiceRequest);
        Task<bool> DeleteTarotServiceAsync(string id);
    }
}
