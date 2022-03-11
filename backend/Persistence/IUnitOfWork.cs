using System.Threading.Tasks;
using webbot.Persistence;

namespace backend.Persistence
{
    public interface IUnitOfWork
    {
        public IPlayerRepository PlayerRepository { get; }
        public ISettingsRepository SettingsRepository { get; }

        Task<bool> CompleteAsync();
    }
}