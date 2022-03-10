using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Persistence;
using webbot.Models;

namespace webbot.Mapping
{
    public class PlayerCookieBoolResolver : IValueResolver<Player, PlayerDto, Boolean>
    {
        public bool Resolve(Player source, PlayerDto destination, bool destMember, ResolutionContext context)
        {
            return source.Cookie == null ? false : true;
        }
    }
}
