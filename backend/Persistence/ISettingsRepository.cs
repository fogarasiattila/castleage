using System.Threading.Tasks;
using webbot.Enums;

namespace webbot.Persistence
{
    public interface ISettingsRepository
    {
        Task<State> GetStartColosseumBattle();
        Task ToggleStartColosseumBattle();
    }
}