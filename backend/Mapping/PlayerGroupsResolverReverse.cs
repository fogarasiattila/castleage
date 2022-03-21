using AutoMapper;
using backend.Persistence;
using System.Collections.Generic;
using webbot.Models;
using webbot.Persistence;

namespace webbot.Mapping
{
    public class PlayerGroupsResolverReverse : IValueResolver<PlayerDto, Player, ICollection<Group>>
    {
        public ICollection<Group> Resolve(PlayerDto source, Player destination, ICollection<Group> destMember, ResolutionContext context)
        {
            destMember = new List<Group>();

            foreach (var member in source.MemberOf)
            {
                destMember.Add(new Group { Id = member });
            }

            return destMember;
        }
    }
}
