using AutoMapper;
using backend.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webbot.Models;

namespace webbot.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>().ForMember(dest => dest.HasCookie, opt => opt.MapFrom<PlayerCookieBoolResolver>());
            CreateMap<Player, Player>();
        }
    }
}
