using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace StriveBot.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        public CommandService CommandService { get; set; }
        public Configuration Configuration { get; set; }

        [Command]
        [Summary("You are here.")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder();

            foreach (var command in CommandService.Commands)
            {
                var doc = new CommandDoc(Configuration.CommandPrefix, command);
                builder.AddField(doc.Signature, doc.Summary);
            }

            await ReplyAsync("Here's a list of commands: ", false, builder.Build());
        }

        [Command("version")]
        [Summary("What Strive patch is the bot up-to-date with?")]
        public async Task VersionAsync()
        {
            await ReplyAsync("1");
        }

        private class CommandDoc
        {
            public string Signature { get; set; }
            public string Summary { get; set; }

            public CommandDoc(char prefix, CommandInfo command)
            {
                var names = command.Aliases
                    .Select(x => $"{prefix}{x}");
                var altNames = ""; //string.Join(' ', names.Skip(1));
                var parameters = string.Join(' ', command.Parameters.Select(p => $"<{p}>"));
                var summaryText = command.Summary ?? "No summary available";
                
                Signature = $"{names.First()} {parameters}";

                Summary = string.IsNullOrWhiteSpace(altNames)
                    ? summaryText
                    : $"Alternatively: {altNames}\n{summaryText}";
            }
        }
    }
}