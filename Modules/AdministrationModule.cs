namespace StriveBot.Modules;

using Discord.Commands;

using StriveBot.Infrastructure;

[Group("admin")]
[RequireOwner]
[ExcludeFromHelpDoc]
public class AdministrationModule : BaseStriveBotModule
{
    public Configuration Configuration { get; set; } = default!;
    public Discord.Interactions.InteractionService InteractionService { get; set; } = default!;

    [Command("register-commands")]
    public async Task RegisterCommandsAsync() => await this.InteractionService.RegisterCommandsToGuildAsync(this.Configuration.GuildId);
}
