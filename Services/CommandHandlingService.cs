using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

using StriveBot.Characters;
using StriveBot.Infrastructure.TypeReaders;

namespace StriveBot.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commandService;
        private readonly Configuration _config;
        private readonly DiscordSocketClient _discordClient;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _commandService = services.GetRequiredService<CommandService>();
            _config = services.GetRequiredService<Configuration>();
            _discordClient = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commandService.CommandExecuted += CommandExecutedAsync;
            _discordClient.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            _commandService.AddTypeReader(typeof(Character), new CharacterTypeReader());
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)
                || message.Source != MessageSource.User)
            {
                return;
            }

            var argPos = 0;
            if (!message.HasCharPrefix(_config.CommandPrefix, ref argPos))
            {
                return;
            }

            await _commandService.ExecuteAsync(
                context: new SocketCommandContext(_discordClient, message),
                argPos,
                _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified || result.IsSuccess)
            {
                return;
            }

            var errorMessage = result.Error.Value switch
            {
                CommandError.BadArgCount => "Too many arguments to perform the command. Try using quotes or removing spaces.",
                CommandError.ParseFailed => $"Error when parsing command. {result.ErrorReason}",
                _ => $"error: {result}"
            };

            await context.Channel.SendMessageAsync(errorMessage);
        }
    }
}