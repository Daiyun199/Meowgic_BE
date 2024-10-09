using AutoMapper;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.ScheduleReader;
using Meowgic.Data.Models.Request.Service;
using Meowgic.Data.Models.Request.Zodiac;
using Meowgic.Data.Models.Response.CardMeaning;
using Meowgic.Data.Models.Response.ScheduleReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<Zodiac, ZodiacRequestDTO>().ReverseMap();
            CreateMap<ZodiacColor, ZodiacRequestDTO>().ReverseMap();
            CreateMap<CategoryRequestDTO,Category>().ReverseMap();
            CreateMap<CardMeaning, CardMeaningResponseDTO>()
           .ForMember(dest => dest.CardName, opt => opt.MapFrom(src => src.Card.Name))
           .ForMember(dest => dest.LinkUrl, opt => opt.MapFrom(src => src.Card.ImgUrl))
           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<TarotService, ServiceRequest>().ReverseMap();
            // Mapping từ Request DTO sang Entity
            CreateMap<ScheduleRequestDTO2, ScheduleReader>();

            // Mapping từ Entity sang Response DTO
            CreateMap<ScheduleReader, ScheduleReaderResponseDTO>();
        }
    }
}
