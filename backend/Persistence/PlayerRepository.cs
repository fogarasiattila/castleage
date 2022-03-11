using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace backend.Persistence
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayerAsync(string username);

        Task<List<Player>> GetPlayersAsync();

        Task NewPlayer(Player player);

        Task<bool> ModifyPlayer(Player player);

        Task Delete(int id);
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

        public async Task<List<Player>> GetPlayersAsync()
        {
            return await this.botContext.Players.OrderBy(p => p.Username).ToListAsync();
        }

        public async Task<Player> GetPlayerAsync(string username)
        {
            var players = await GetPlayersAsync();
            return players.SingleOrDefault(p => p.Username == username);
        }

        public async Task Delete(int id)
        {
            botContext.Players.Remove(await botContext.Players.FindAsync(id));
        }
    }
}