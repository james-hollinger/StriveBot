namespace StriveBot.Modules;

using Discord;
using Discord.Commands;

public abstract class BaseStriveBotModule : ModuleBase<SocketCommandContext>
{
    protected async Task ReplyWithEmbeddedUrlAsync(string discription, string url)
    {
        var builder = new EmbedBuilder();
        builder.WithDescription($"[{discription}]({url})");
        await this.ReplyAsync("", false, builder.Build());
    }
}
