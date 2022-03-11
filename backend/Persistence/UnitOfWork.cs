using AutoMapper;
using System.Threading.Tasks;
using webbot.Persistence;

namespace backend.Persistence
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly BotContext botContext;
        private readonly IMapper mapper;

        public UnitOfWork(BotContext botContext, IMapper mapper)
        {
            this.botContext = botContext;
            this.mapper = mapper;
        }

        public IPlayerRepository PlayerRepository => new PlayerRepository(botContext, mapper);
        public ISettingsRepository SettingsRepository => new SettingsRepository(botContext);


        public async Task<bool> CompleteAsync()
        {
            return await this.botContext.SaveChangesAsync() > 0;
        }
    }
}