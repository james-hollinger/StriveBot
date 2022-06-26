namespace StriveBot.Services;

using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public class InteractionHandlingService
{
    private readonly DiscordSocketClient discordClient;
    private readonly InteractionService interactionService;
    private readonly IServiceProvider services;

    public InteractionHandlingService(IServiceProvider services)
    {
        this.discordClient = services.GetRequiredService<DiscordSocketClient>();
        this.interactionService = services.GetRequiredService<InteractionService>();
        this.services = services;

        this.discordClient.ButtonExecuted += this.MessageComponentExecutedAsync;
        this.discordClient.InteractionCreated += this.InteractionCreatedAsync;
        this.discordClient.MessageCommandExecuted += this.MessageCommandExecutedAsync;
        this.discordClient.SelectMenuExecuted += this.MessageComponentExecutedAsync;
    }

    public async Task InitializeAsync() => await this.interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), this.services);

    private async Task InteractionCreatedAsync(SocketInteraction interaction)
    {
        var ctx = new SocketInteractionContext(this.discordClient, interaction);
        await this.interactionService.ExecuteCommandAsync(ctx, this.services);
    }

    private async Task MessageCommandExecutedAsync(SocketMessageCommand command)
    {
        var ctx = new SocketInteractionContext<SocketMessageCommand>(this.discordClient, command);
        await this.interactionService.ExecuteCommandAsync(ctx, this.services);
    }

    private async Task MessageComponentExecutedAsync(SocketMessageComponent component)
    {
        var ctx = new SocketInteractionContext<SocketMessageComponent>(this.discordClient, component);
        await this.interactionService.ExecuteCommandAsync(ctx, this.services);
    }
}
