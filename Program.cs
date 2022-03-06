namespace StriveBot;

using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading;

using StriveBot.Services;

public class Program
{
    private static void Main(string[] _) => new Program().MainAsync().GetAwaiter().GetResult();

    public async Task MainAsync()
    {
        using var services = ConfigureServices();
        services.GetRequiredService<CommandService>().Log += this.LogAsync;
        await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
        await services.GetRequiredService<InteractionHandlingService>().InitializeAsync();

        var client = services.GetRequiredService<DiscordSocketClient>();
        client.Log += this.LogAsync;

        await client.LoginAsync(TokenType.Bot, services.GetRequiredService<Configuration>().DiscordBotToken);
        await client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }

    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());

        return Task.CompletedTask;
    }

    private static ServiceProvider ConfigureServices()
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
            .AddSingleton(config.Get<Configuration>())
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<HttpClient>()
            .AddSingleton<InteractionHandlingService>()
            .AddSingleton<InteractionService>()
            .AddSingleton<PersistenceService>()
            .BuildServiceProvider();
    }
}
