using backend.Persistence;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using webbot.Consts;
using webbot.Enums;
using System.Threading.Tasks;

namespace webbot.Persistence
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly BotContext botContext;

        public SettingsRepository(BotContext botContext)
        {
            this.botContext = botContext;
        }

        public async Task<State> GetStartColosseumBattle()
        {
            return (await botContext.Settings.FirstOrDefaultAsync(r => r.Name == DatabaseConsts.StartColosseumBattleSetting)).State;
        }

        public async Task ToggleStartColosseumBattle()
        {
            var setting = await botContext.Settings.FirstOrDefaultAsync(r => r.Name == DatabaseConsts.StartColosseumBattleSetting);
            setting.State = setting.State == State.Off ? State.On : State.Off;
            botContext.Update(setting);
        }
    }
}
