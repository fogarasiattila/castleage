using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Persistence;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using webbot.Controllers;
using webbot.Services;

namespace botservice
{
    public class ColosseumBattleService : BackgroundService
    {
        private readonly ILogger<ColosseumBattleService> _logger;
        private readonly IParseHtml parseHtml;

        public ColosseumBattleService(ILogger<ColosseumBattleService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(1000, stoppingToken);

                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                TimeSpan colosseumOpeningTime = new TimeSpan(15, 0, 0);
                TimeSpan colosseumClosingTime = new TimeSpan(22, 0, 0);

                if ((currentTime - colosseumOpeningTime > TimeSpan.Zero) && (currentTime - colosseumClosingTime < TimeSpan.Zero))
                {
                    await StartBattle();
                }
                else
                {
                    TimeSpan sleeping = new TimeSpan(1, 16, 1, 0) - DateTime.Now.TimeOfDay;
                    Console.WriteLine($"Waiting {sleeping} for next battle...");
                    Thread.Sleep(sleeping);
                }
            }
        }

        private async Task StartBattle()
        {

        }

    }
}
