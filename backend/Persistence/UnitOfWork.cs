using System.Threading.Tasks;

namespace backend.Persistence
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly BotContext botContext;

        public UnitOfWork(BotContext botContext)
        {
            this.botContext = botContext;
        }
        public async Task CompleteAsync()
        {
            await this.botContext.SaveChangesAsync();
        }
    }
}