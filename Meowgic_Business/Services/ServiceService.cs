using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Service;
using Meowgic.Data.Repositories;
using Meowgic.Shares.Exceptions;
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
        private readonly IAccountRepository _accountRepository;
        public TarotServiceService(IServiceRepository tarotServiceRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IAccountRepository accountRepository)
        {
            _serviceRepository = tarotServiceRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _accountRepository = accountRepository;
        }

        public async Task<TarotService> CreateTarotServiceAsync(ServiceRequest tarotServiceRequest, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var tarotService = _mapper.Map<TarotService>(tarotServiceRequest);
            tarotService.AccountId = userId;
            return await _serviceRepository.CreateTarotServiceAsync(tarotService);
        }

        public async Task<TarotService?> GetTarotServiceByIdAsync(string id)
        {
            return await _serviceRepository.GetTarotServiceByIdAsync(id);
        }
        public async Task<List<TarotService>> GetTarotServiceByAccountIdAsync(string id)
        {
            return await _serviceRepository.GetTarotServicesByAccountIdAsync(id);
        }

        public async Task<IEnumerable<TarotService>> GetAllTarotServicesAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var test = userId;
            return await _serviceRepository.GetAllTarotServicesAsync();
        }

        public async Task<TarotService?> UpdateTarotServiceAsync(string id, ServiceRequest tarotServiceRequest, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var existingService = _serviceRepository.GetTarotServiceByIdAsync(id);
            if (existingService is null)
            {
                throw new BadHttpRequestException("Not found service");
            }
            var tarotService = _mapper.Map<TarotService>(tarotServiceRequest);
            if (tarotService.AccountId != userId)
            {
                throw new ForbiddenMethodException("You can't edit this service");
            }
            return await _serviceRepository.UpdateTarotServiceAsync(id, tarotService);
        }

        public async Task<bool> DeleteTarotServiceAsync(string id, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var existingService = _serviceRepository.GetTarotServiceByIdAsync(id);
            if (existingService is null)
            {
                throw new BadHttpRequestException("Not found service");
            }
            var tarotService = _mapper.Map<TarotService>(existingService);
            if (tarotService.AccountId != userId)
            {
                throw new ForbiddenMethodException("You can't remove this service");
            }
            return await _serviceRepository.DeleteTarotServiceAsync(id);
        }
    }
}
