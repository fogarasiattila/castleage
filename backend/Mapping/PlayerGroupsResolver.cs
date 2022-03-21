using AutoMapper;
using backend.Persistence;
using System.Collections.Generic;
using webbot.Models;

namespace webbot.Mapping
{
    public class PlayerGroupsResolver : IValueResolver<Player, PlayerDto, List<int>>
    {
        public List<int> Resolve(Player source, PlayerDto destination, List<int> destMember, ResolutionContext context)
        {
            destMember = new();
            
            foreach (var group in source.Groups)
            {
                destMember.Add(group.Id);
            }

            return destMember;
        }
    }
}
