using backend.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webbot.Services
{
    public class PlayerRepositoryInterface
    {
        private readonly IPlayerRepository playerRepository;

        public PlayerRepositoryInterface(IServiceProvider serviceProvider)
        {
            playerRepository = serviceProvider.GetRequiredService<IPlayerRepository>();
        }

        public async Task<List<Player>> GetPlayersAsync()
        {
            return await playerRepository.GetPlayersAsync();
        }
    }
}
