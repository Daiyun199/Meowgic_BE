using AutoMapper;
using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Zodiac;
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
        }
    }
}
