using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace StriveBot.Modules
{
    public abstract class BaseStriveBotModule : ModuleBase<SocketCommandContext>
    {
        protected async Task ReplyWithEmbeddedUrlAsync(string discription, string url)
        {
            var builder = new EmbedBuilder();
            builder.WithDescription($"[{discription}]({url})");
            await ReplyAsync("", false, builder.Build());
        }
    }
}