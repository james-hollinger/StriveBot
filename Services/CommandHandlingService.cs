namespace StriveBot.Services;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using StriveBot.Models.Characters;
using StriveBot.Infrastructure.TypeReaders;

public class CommandHandlingService
{
    private readonly CommandService commandService;
    private readonly Configuration config;
    private readonly DiscordSocketClient discordClient;
    private readonly IServiceProvider services;

    public CommandHandlingService(IServiceProvider services)
    {
        this.commandService = services.GetRequiredService<CommandService>();
        this.config = services.GetRequiredService<Configuration>();
        this.discordClient = services.GetRequiredService<DiscordSocketClient>();
        this.services = services;

        this.commandService.CommandExecuted += CommandExecutedAsync;
        this.discordClient.MessageReceived += this.MessageReceivedAsync;
    }

    public async Task InitializeAsync()
    {
        this.commandService.AddTypeReader(typeof(Character), new CharacterTypeReader());
        await this.commandService.AddModulesAsync(Assembly.GetEntryAssembly(), this.services);
    }

    public async Task MessageReceivedAsync(SocketMessage rawMessage)
    {
        // Ignore system messages, or messages from other bots
        if (rawMessage is not SocketUserMessage message
            || message.Source != MessageSource.User)
        {
            return;
        }

        var argPos = 0;
        if (!message.HasCharPrefix(this.config.CommandPrefix, ref argPos))
        {
            return;
        }

        await this.commandService.ExecuteAsync(
            context: new SocketCommandContext(this.discordClient, message),
            argPos,
            this.services);
    }

    public static async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
        // command is unspecified when there was a search failure (command not found); we don't care about these errors
        if (!command.IsSpecified || result.IsSuccess)
        {
            return;
        }

        var errorMessage = result.Error!.Value switch
        {
            CommandError.BadArgCount => "Too many arguments to perform the command. Try using quotes or removing spaces.",
            CommandError.ParseFailed => $"Error when parsing command. {result.ErrorReason}",
            _ => $"error: {result}"
        };

        await context.Channel.SendMessageAsync(errorMessage);
    }
}
