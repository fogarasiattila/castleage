using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Persistence;
using backend.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using webbot.Services;

namespace botservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ColosseumBattleService>();
                });
    }
}
