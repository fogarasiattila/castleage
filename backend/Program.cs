using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using backend.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace webbot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

            

            try
            {
                Log.Information("BOT indítása...");
                var host = CreateWebHostBuilder(args).Build();

                //migráció
                using var scope = host.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<BotContext>();
                db.Database.Migrate();

                host.Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseSerilog()
                .UseStartup<Startup>().UseKestrel();
    }
}
