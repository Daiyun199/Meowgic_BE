using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Zodiac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class ZodiacService(IZodiacRepository zodiacRepository, IMapper mapper) : IZodiacService
    {
        private readonly IZodiacRepository _zodiacRepository = zodiacRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Zodiac> CreateZodiacAsync(ZodiacRequestDTO zodiacDto)
        {
            var zodiacExist = await _zodiacRepository.GetZodiacByNameAsync(zodiacDto.Name);
            if(zodiacExist != null)
            {
                throw new Exception($"Zodiac with Name:  ( {zodiacDto.Name} ) is exist");

            }
            var zodiacEntity = _mapper.Map<Zodiac>(zodiacDto); 
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
        public async Task<Zodiac?> UpdateZodiacAsync(string id, ZodiacRequestDTO zodiacDto)
        {
            var existingZodiac = await _zodiacRepository.GetZodiacByIdAsync(id);
            if (existingZodiac != null)
            {
                _mapper.Map(zodiacDto, existingZodiac);
                var updatedZodiac = await _zodiacRepository.UpdateZodiacAsync(existingZodiac);
                return updatedZodiac;
            }

            throw new Exception($"Zodiac with ID ( {id} ) NOT FOUND");
        }

        // Delete
        public async Task<bool> DeleteZodiacAsync(string id)
        {
            var existingZodiac = await _zodiacRepository.GetZodiacByIdAsync(id);
            if (existingZodiac != null)
            {
                return await _zodiacRepository.DeleteZodiacAsync(id);
            }
            throw new Exception($"Zodiac with ID ( {id} ) NOT FOUND");
        }
    }
}
