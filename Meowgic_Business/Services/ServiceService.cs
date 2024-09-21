using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Service;
using Meowgic.Data.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class TarotServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TarotServiceService(IServiceRepository tarotServiceRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _serviceRepository = tarotServiceRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TarotService> CreateTarotServiceAsync(ServiceRequest tarotServiceRequest)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tarotService = _mapper.Map<TarotService>(tarotServiceRequest);
            tarotService.AccountId = userId;
            return await _serviceRepository.CreateTarotServiceAsync(tarotService);
        }

        public async Task<TarotService?> GetTarotServiceByIdAsync(string id)
        {
            return await _serviceRepository.GetTarotServiceByIdAsync(id);
        }

        public async Task<IEnumerable<TarotService>> GetAllTarotServicesAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var test = userId;
            return await _serviceRepository.GetAllTarotServicesAsync();
        }

        public async Task<TarotService?> UpdateTarotServiceAsync(string id, ServiceRequest tarotServiceRequest)
        {
            var tarotService = _mapper.Map<TarotService>(tarotServiceRequest);
            return await _serviceRepository.UpdateTarotServiceAsync(id, tarotService);
        }

        public async Task<bool> DeleteTarotServiceAsync(string id)
        {
            return await _serviceRepository.DeleteTarotServiceAsync(id);
        }
    }
}
