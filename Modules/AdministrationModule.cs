namespace StriveBot.Modules;

using Discord.Commands;
using Newtonsoft.Json;

using StriveBot.Infrastructure;

[Group("admin")]
[RequireOwner]
[ExcludeFromHelpDoc]
public class AdministrationModule : BaseStriveBotModule
{
    public Discord.Interactions.InteractionService InteractionService { get; set; } = default!;

    [Command("register-commands")]
    public async Task RegisterCommandsAsync()
    {
        var commandInfo = await this.InteractionService.RegisterCommandsGloballyAsync();
        Console.WriteLine(JsonConvert.SerializeObject(commandInfo));
    }
}