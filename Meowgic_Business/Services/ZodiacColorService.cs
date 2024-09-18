using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.ZodiacColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class ZodiacColorService : IZodiacColorService
    {
        private readonly IZodiacColorRepository _zodiacColorRepository;
        private readonly IMapper _mapper;
        private readonly IZodiacRepository _zodiacRepository;
        public ZodiacColorService(IZodiacColorRepository zodiacColorRepository, IMapper mapper, IZodiacRepository zodiacRepository)
        {
            _zodiacColorRepository = zodiacColorRepository;
            _mapper = mapper;
            _zodiacRepository = zodiacRepository;
        }

        // Create
        public async Task<ZodiacColor> CreateZodiacColorAsync(ZodiacColorRequestDTO zodiacColorDto)
        {
            var zodiacExist =await _zodiacRepository.GetZodiacByIdAsync(zodiacColorDto.ZodiacId);
            if(zodiacExist == null) {
                throw new Exception($"Zodiac with ID ( {zodiacColorDto.ZodiacId} ) NOT FOUND");
            }
            var zodiacColorEntity = _mapper.Map<ZodiacColor>(zodiacColorDto);
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
                throw new Exception($"Zodiac with ID ( {id} ) NOT FOUND");
            }
            return zodiacColor;
        }
        // Read (Get all)
        public async Task<IEnumerable<ZodiacColor>> GetAllZodiacColorsAsync()
        {
            return await _zodiacColorRepository.GetAllZodiacColorsAsync();
        }

        // Update
        public async Task<ZodiacColor?> UpdateZodiacColorAsync(string id, ZodiacColorRequestDTO zodiacColorDto)
        {
            var existingZodiacColor = await _zodiacColorRepository.GetZodiacColorByIdAsync(id);
            if (existingZodiacColor == null)
            {
                throw new Exception($"ZodiacColor with ID ( {id} ) NOT FOUND");
            }

            _mapper.Map(zodiacColorDto, existingZodiacColor);
            var updatedZodiacColor = await _zodiacColorRepository.UpdateZodiacColorAsync(existingZodiacColor);
            return updatedZodiacColor;
        }

        // Delete
        public async Task<bool> DeleteZodiacColorAsync(string id)
        {
            var existingZodiacColor = await _zodiacColorRepository.GetZodiacColorByIdAsync(id);
            if (existingZodiacColor == null)
            {
                throw new Exception($"ZodiacColor with ID ( {id} ) NOT FOUND");
            }
            return await _zodiacColorRepository.DeleteZodiacColorAsync(id);
        }
    }
}
