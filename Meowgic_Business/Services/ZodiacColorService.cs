using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.ZodiacColor;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class ZodiacColorService : IZodiacColorService
    {
        private readonly IZodiacColorRepository _zodiacColorRepository;
        private readonly IMapper _mapper;
        private readonly IZodiacRepository _zodiacRepository;
        private readonly IAccountRepository _accountRepository;
        public ZodiacColorService(IZodiacColorRepository zodiacColorRepository, IMapper mapper, IZodiacRepository zodiacRepository, IAccountRepository accountRepository)
        {
            _zodiacColorRepository = zodiacColorRepository;
            _mapper = mapper;
            _zodiacRepository = zodiacRepository;
            _accountRepository = accountRepository;
        }

        // Create
        public async Task<ZodiacColor> CreateZodiacColorAsync(ZodiacColorRequestDTO zodiacColorDto, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var zodiacExist =await _zodiacRepository.GetZodiacByIdAsync(zodiacColorDto.ZodiacId);
            if(zodiacExist == null) {
                throw new Exception($"Zodiac with ID ( {zodiacColorDto.ZodiacId} ) NOT FOUND");
            }
            var zodiacColorEntity = _mapper.Map<ZodiacColor>(zodiacColorDto);
            zodiacColorEntity.CreatedBy = userId;
            zodiacColorEntity.CreatedTime = DateTime.Now;
            var createdZodiacColor = await _zodiacColorRepository.CreateZodiacColorAsync(zodiacColorEntity);
            return createdZodiacColor;
        }

        // Read (Get by ID)
        public async Task<ZodiacColor?> GetZodiacColorByIdAsync(string id)
        {
            var zodiacColor = await _zodiacColorRepository.GetZodiacColorByIdAsync(id);
            if (zodiacColor == null)
            {
                throw new Exception($"ZodiacColor with ID ( {id} ) NOT FOUND");
            }
            return zodiacColor;
        }

        public async Task<ZodiacColor?> GetZodiacColorByZodiacIdAsync(string id)
        {

            var zodiacExist = await _zodiacRepository.GetZodiacByIdAsync(id);
            if (zodiacExist == null)
            {
                throw new Exception($"Zodiac with ID ( {id} ) NOT FOUND");
            }
            var zodiacColor = await _zodiacColorRepository.GetZodiacColorByZodiacIdAsync(id);
            if (zodiacColor == null)
            {
                throw new Exception($"Zodiac color with ID ( {id} ) NOT FOUND");
            }
            return zodiacColor;
        }
        // Read (Get all)
        public async Task<IEnumerable<ZodiacColor>> GetAllZodiacColorsAsync()
        {
            return await _zodiacColorRepository.GetAllZodiacColorsAsync();
        }

        // Update
        public async Task<ZodiacColor?> UpdateZodiacColorAsync(string id, ZodiacColorRequestDTO zodiacColorDto, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var existingZodiacColor = await _zodiacColorRepository.GetZodiacColorByIdAsync(id);
            if (existingZodiacColor == null)
            {
                throw new Exception($"ZodiacColor with ID ( {id} ) NOT FOUND");
            }

            existingZodiacColor.LastUpdatedTime = DateTime.Now;
            existingZodiacColor.LastUpdatedBy = userId;

            _mapper.Map(zodiacColorDto, existingZodiacColor);

            var updatedZodiacColor = await _zodiacColorRepository.UpdateZodiacColorAsync(existingZodiacColor);
            return updatedZodiacColor;
        }

        // Delete
        public async Task<bool> DeleteZodiacColorAsync(string id, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var existingZodiacColor = await _zodiacColorRepository.GetZodiacColorByIdAsync(id);
            if (existingZodiacColor == null)
            {
                throw new Exception($"ZodiacColor with ID ( {id} ) NOT FOUND");
            }
            existingZodiacColor.DeletedBy = userId;
            existingZodiacColor.DeletedTime = DateTime.Now;
            await _zodiacColorRepository.UpdateZodiacColorAsync(existingZodiacColor);
            return true;
        }
    }
}
