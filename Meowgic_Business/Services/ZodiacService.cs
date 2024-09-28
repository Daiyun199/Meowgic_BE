using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Zodiac;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class ZodiacService(IZodiacRepository zodiacRepository, IMapper mapper, IAccountRepository accountRepository) : IZodiacService
    {
        private readonly IZodiacRepository _zodiacRepository = zodiacRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<Zodiac> CreateZodiacAsync(ZodiacRequestDTO zodiacDto, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var zodiacExist = await _zodiacRepository.GetZodiacByNameAsync(zodiacDto.Name);
            if(zodiacExist != null)
            {
                throw new Exception($"Zodiac with Name:  ( {zodiacDto.Name} ) is exist");

            }
            var zodiacEntity = _mapper.Map<Zodiac>(zodiacDto); 
            zodiacEntity.CreatedBy = userId;
            zodiacEntity.CreatedTime = DateTime.Now;
            var createdZodiac = await _zodiacRepository.CreateZodiacAsync(zodiacEntity);
            return createdZodiac;  
        }

        // Read (Get by ID)
        public async Task<Zodiac?> GetZodiacByIdAsync(string id)
        {
            var zodiac = _zodiacRepository.GetZodiacByIdAsync(id).Result;
            if (zodiac != null)
            {
                return zodiac;
            }// Trả về entity Zodiac
            throw new Exception($"Zodiac with ID ( {id} ) NOT FOUND");
        }

        // Read (Get all)
        public async Task<IEnumerable<Zodiac>> GetAllZodiacsAsync()
        {
            return await _zodiacRepository.GetAllZodiacsAsync();  
        }

        // Update
        public async Task<Zodiac?> UpdateZodiacAsync(string id, ZodiacRequestDTO zodiacDto, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var existingZodiac = await _zodiacRepository.GetZodiacByIdAsync(id);
            if (existingZodiac != null)
            {
                existingZodiac.LastUpdatedTime = DateTime.Now;
                existingZodiac.LastUpdatedBy = userId;
                _mapper.Map(zodiacDto, existingZodiac);
                var updatedZodiac = await _zodiacRepository.UpdateZodiacAsync(existingZodiac);
                return updatedZodiac;
            }

            throw new Exception($"Zodiac with ID ( {id} ) NOT FOUND");
        }

        // Delete
        public async Task<bool> DeleteZodiacAsync(string id, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var existingZodiac = await _zodiacRepository.GetZodiacByIdAsync(id);
            if (existingZodiac != null)
            {
                existingZodiac.DeletedTime = DateTime.Now;
                existingZodiac.DeletedBy = userId;
                await _zodiacRepository.UpdateZodiacAsync(existingZodiac);
                return true;
            }
            throw new Exception($"Zodiac with ID ( {id} ) NOT FOUND");
        }
    }
}
