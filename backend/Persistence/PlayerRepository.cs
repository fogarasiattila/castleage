using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using webbot.Persistence;
using webbot.Enums;

namespace backend.Persistence
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayerByNameAsync(string username);
        Player GetPlayerByName(string username);

        Task<List<Player>> GetPlayersAsync();

        Task NewPlayer(Player player);

        Task<bool> ModifyPlayer(Player player);

        Task DeletePlayerAsync(int id);

        Task<List<Group>> GetGroupsAsync();
        Group GetGroupByName(string name);
        Group GetGroupById(int id);

        Task<List<Player>> GetPlayersByGroupIdAsync(int id);
        Task<List<Player>> GetPlayersByGroupNameAsync(string name);
        void MovePlayerToGroup(Player player, Group srcGroup, Group dstGroup);
        void AddGroup(Group group);
        void DeleteGroup(Group group);
    }


    public class PlayerRepository : IPlayerRepository
    {
        private readonly BotContext botContext;
        private readonly IMapper mapper;

        public PlayerRepository(BotContext botContext, IMapper mapper)
        {
            this.botContext = botContext;
            this.mapper = mapper;
        }

        //cache control!
        public async Task NewPlayer(Player player)
        {
            var players = await GetPlayersAsync();

            if (players.SingleOrDefault(p => p.Username == player.Username) == null)
            {
                botContext.Players.Add(player);
            }
        }

        public async Task<bool> ModifyPlayer(Player playerDto)
        {

            var player = await botContext.Players.FindAsync(playerDto.Id);

            if (player == null) return false;

            botContext.Update(mapper.Map<Player, Player>(playerDto, player));

            return true;
        }

        public Task<List<Player>> GetPlayersAsync()
        {
            return this.botContext.Players.OrderBy(p => p.Username).Include(p => p.Groups).ToListAsync();
        }

        public Task<Player> GetPlayerByNameAsync(string username)
        {
            return botContext.Players.FirstOrDefaultAsync(p => p.Username == username);
        }

        public async Task DeletePlayerAsync(int id)
        {
            botContext.Players.Remove(await botContext.Players.FindAsync(id));
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            return await botContext.Groups.ToListAsync();
        }

        public Task<List<Player>> GetPlayersByGroupNameAsync(string name)
        {
            return botContext.Players.Include(p => p.Groups).Where(p => p.Groups.Any(g => g.Name == name)).ToListAsync();
        }

        public Task<List<Player>> GetPlayersByGroupIdAsync(int id)
        {
            return botContext.Players.Include(p => p.Groups).Where(p => p.Groups.Any(g => g.Id == id)).ToListAsync();
        }

        public Group GetGroupByName(string name)
        {
            return botContext.Groups.FirstOrDefault(g => g.Name == name);
        }

        public Group GetGroupById(int id)
        {
            return botContext.Groups.FirstOrDefault(g => g.Id == id);
        }

        public Player GetPlayerByName(string username)
        {
            return botContext.Players.Include(p => p.Groups).FirstOrDefault(p => p.Username == username);
        }

        public void DeleteGroup(Group group)
        {
            botContext.Groups.Remove(group);
        }

        public void AddGroup(Group group)
        {
            botContext.Groups.Add(group);
        }

        public void MovePlayerToGroup(Player player, Group srcGroup, Group dstGroup)
        {
            if (srcGroup.Id != (int)Groups.Mindenki) player.Groups.Remove(srcGroup);
            if (dstGroup.Id != (int)Groups.Mindenki) player.Groups.Add(dstGroup);
        }

    }
}