using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Persistence;
using backend.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using webbot.Controllers;
using webbot.Services;

namespace botservice
{
    public class ColosseumBattleService : BackgroundService
    {
        private readonly ILogger<ColosseumBattleService> _logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private IUnitOfWork uow;
        private readonly CultureInfo provider = CultureInfo.InvariantCulture;
        private List<Task<Player>> battleTasks = new List<Task<Player>>();

        TimeSpan colosseumOpeningTimeMorning = new TimeSpan(5, 0, 0);
        TimeSpan colosseumClosingTimeMorning = new TimeSpan(8, 0, 0);
        TimeSpan colosseumOpeningTimeEvening = new TimeSpan(19, 0, 0);
        TimeSpan colosseumClosingTimeEvening = new TimeSpan(22, 0, 0);

        public ColosseumBattleService(ILogger<ColosseumBattleService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();
            var players = await playerRepository.GetPlayersAsync();
            uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            while (!cancellationToken.IsCancellationRequested)
            {
                TimeSpan currentTime = DateTime.Now.TimeOfDay;

                if (WithinDueTime())
                {
                    await StartBattle(players, cancellationToken);
                }
                else
                {
                    TimeSpan sleeping;
                    if (currentTime - colosseumOpeningTimeMorning < TimeSpan.Zero) sleeping = colosseumOpeningTimeMorning - currentTime;
                    else
                    if (currentTime - colosseumOpeningTimeEvening < TimeSpan.Zero) sleeping = colosseumOpeningTimeEvening - currentTime;
                    else sleeping = colosseumOpeningTimeMorning.Add(TimeSpan.FromDays(1)) - DateTime.Now.TimeOfDay;
                    _logger.LogWarning($"BOT: {sleeping} id� m�lva kezd�dik a k�vetkez� csata, addig v�runk...");

                    await Task.Delay(sleeping);
                }
            }
        }

        private async Task StartBattle(List<Player> players, CancellationToken cancellationToken)
        {
            _logger.LogInformation("BOT: StartBattle job indul.");

            for (int i = 0; i < players.Count; i++)
            {
                await DispatchPlayerToBattle(players[i]);
            }

            while (battleTasks.Count > 0)
            {
                var finishedTask = await Task.WhenAny(battleTasks);
                battleTasks.Remove(finishedTask);
                _logger.LogDebug($"A battle task has finished. {battleTasks.Count} remaining.");

                if (WithinDueTime())
                {
                    await DispatchPlayerToBattle(await finishedTask);
                }
            }

            async Task DispatchPlayerToBattle(Player player)
            {
                using var scope = serviceScopeFactory.CreateScope();
                var callCastle = scope.ServiceProvider.GetRequiredService<ICallCastle>();
                callCastle.Player = player;
                await LoginOnDemandAsync(callCastle);
                player.InBattle = true;
                //battleTasks.Add(callCastle.DummyAsync());
                battleTasks.Add(callCastle.ColosseumAsync());
            }
        }

        private async Task LoginOnDemandAsync(ICallCastle callCastle)
        {
            if (callCastle.Player.Cookie != null)
            {
                var index = callCastle.Player.Cookie.IndexOf(",") + 2;
                var dateString = callCastle.Player.Cookie.Substring(index, 20);
                var format = "dd-MMM-yyyy HH:mm:ss";

                try
                {
                    var result = DateTime.ParseExact(dateString, format, provider);

                    var now = DateTime.Now;

                    if (now < result) return;
                }
                catch (Exception)
                {
                    _logger.LogError("Nem siker�lt a cookie lej�rati d�tum�nak konvert�l�sa d�tumm�!");
                }
            }

            _logger.LogInformation($"{callCastle.Player.Username}: �j cookie-t k�r�nk...");
            if (await callCastle.LoginAsync()) await uow.CompleteAsync();
        }

        private bool WithinDueTime()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            return (currentTime - colosseumOpeningTimeMorning >= TimeSpan.Zero) && (currentTime - colosseumClosingTimeMorning <= TimeSpan.Zero) ||
                    (currentTime - colosseumOpeningTimeEvening >= TimeSpan.Zero) && (currentTime - colosseumClosingTimeEvening <= TimeSpan.Zero);
        }

    }
}
