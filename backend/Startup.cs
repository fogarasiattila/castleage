using System;
using System.Net.Http;
using backend.Persistence;
using backend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using webbot.Services;
using Microsoft.AspNetCore.Mvc;
using webbot.Models;
using botservice;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace webbot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string CorsAllowedOriginsPolicyName = "MyAllowedDomains";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: FACTORY?
            //https://espressocoder.com/2018/10/08/injecting-a-factory-service-in-asp-net-core/
            //services.AddHostedService<ColosseumBattleService>();
            //services.AddControllers().AddNewtonsoftJson(conf => conf.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddSingleton<ColosseumBattleService>();
            services.AddSingleton<IHostedService>( sp => sp.GetRequiredService<ColosseumBattleService>() );
            services.AddMvc();//.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpContextAccessor();
            services.AddSingleton<IParseHtml, ParseHtml>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<CookieDelegatingHandler>();
            services.AddDbContext<BotContext>(options => 
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
                .LogTo(System.Console.Write)
                );
            //services.AddDbContext<BotContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddHttpClient<ICallCastle, CallCastle>(config =>
            {
                config.BaseAddress = new Uri("https://web3.castleagegame.com");
            }).SetHandlerLifetime(TimeSpan.FromHours(3)).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new SocketsHttpHandler()
                {
                    AllowAutoRedirect = false,
                    UseCookies = false,

                };
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {

                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (r, c, ch, s) => true,
                    AutomaticDecompression = System.Net.DecompressionMethods.All,
                    AllowAutoRedirect = false
                };

                return handler;
            })
            ;
            services.AddCors(o =>
            {
                o.AddPolicy(CorsAllowedOriginsPolicyName, builder =>
                {
                    builder.WithOrigins().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                }
                );
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
            app.UseCors(CorsAllowedOriginsPolicyName);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
                //endpoints.MapControllers().RequireCors(CorsAllowedOriginsPolicyName);

            });
                
        }
    }
}
