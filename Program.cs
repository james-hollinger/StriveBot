using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using StriveBot.Services;

namespace StriveBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                services.GetRequiredService<CommandService>().Log += LogAsync;
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                var client = services.GetRequiredService<DiscordSocketClient>();
                client.Log += LogAsync;

                await client.LoginAsync(TokenType.Bot, services.GetRequiredService<Configuration>().DiscordBotToken);
                await client.StartAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables()
                .Build();

            return new ServiceCollection()
                .AddSingleton<CharacterService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<CommandService>()
                .AddSingleton<Configuration>(config.Get<Configuration>())
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<HttpClient>()
                .AddSingleton<PersistenceService>()
                .BuildServiceProvider();
        }
    }
}