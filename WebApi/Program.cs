/* proffesor used a different way to start the api, this one is the default
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();*/

using BusinessLogic.Data;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();//.Run();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<NetMarketDbContext>();
                    await context.Database.MigrateAsync();

                    await NetMarketDbContextData.LoadDataAsync(context, loggerFactory);

                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var identityContext = services.GetRequiredService<SecurityDbContext>();

                    await identityContext.Database.MigrateAsync();
                    await SecurityDbContextData.SeedUserAsync(userManager, loggerFactory);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "The migration process failed");
                }
            }

            host.Run();

        }

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    }
}
