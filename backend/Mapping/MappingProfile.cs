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
            CreateMap<Player, PlayerDto>()
                .ForMember(dest => dest.HasCookie, opt => opt.MapFrom<PlayerCookieBoolResolver>())
                .ForMember(dest => dest.MemberOf, opt => opt.MapFrom<PlayerGroupsResolver>());

            CreateMap<PlayerDto, Player>()
                .ForMember(dest => dest.Groups, opt => opt.MapFrom<PlayerGroupsResolverReverse>());

            CreateMap<Player, Player>();
        }
    }
}
