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

        void ResetCache();
    }


    public class PlayerRepository : IPlayerRepository
    {
        private readonly BotContext botContext;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;

        public PlayerRepository(BotContext botContext, IUnitOfWork uow, IMapper mapper, IMemoryCache memoryCache)
        {
            this.botContext = botContext;
            this.uow = uow;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
        }

        //cache control!
        public async Task NewPlayer(Player player)
        {
            var players = await GetPlayersAsync();

            if (players.SingleOrDefault(p => p.Username == player.Username) == null)
            {
                botContext.Players.Add(player);
                await this.uow.CompleteAsync();
            }

            //ResetCache();
        }

        //cache control!
        public async Task<bool> ModifyPlayer(Player playerDto)
        {

            var player = await botContext.Players.FindAsync(playerDto.Id);

            if (player == null) return false;

            mapper.Map<Player, Player>(playerDto, player);

            await uow.CompleteAsync();

            //ResetCache();

            return true;
        }

        public async Task<List<Player>> GetPlayersAsync()
        {
            //if (!memoryCache.TryGetValue("Players", out List<Player> players))
            //{
            //    memoryCache.Set("Players", await this.botContext.Players.ToListAsync());
            //}
            //return memoryCache.Get("Players") as List<Player>;
            return await this.botContext.Players.OrderBy(p => p.Username).ToListAsync();
        }

        public async Task<Player> GetPlayerAsync(string username)
        {
            var players = await GetPlayersAsync();
            return players.SingleOrDefault(p => p.Username == username);
        }

        //cache control!
        public async Task Delete(int id)
        {
            botContext.Players.Remove(botContext.Players.Find(id));
            await uow.CompleteAsync();
            
            //ResetCache();
        }

        public void ResetCache()
        {
            memoryCache.Remove("Players");
        }
    }
}