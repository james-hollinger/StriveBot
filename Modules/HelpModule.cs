namespace StriveBot.Modules;

using Discord;
using Discord.Commands;

using StriveBot.Services;

[Group("help")]
[Summary("Documentation of StriveBot's commands.")]
public class HelpModule : BaseStriveBotModule
{
    public CharacterService CharacterService { get; set; } = default!;
    public CommandService CommandService { get; set; } = default!;
    public Configuration Configuration { get; set; } = default!;

    [Command]
    [Summary("You are here.")]
    public async Task HelpAsync()
    {
        var builder = new EmbedBuilder();

        foreach (var command in this.CommandService.Commands)
        {
            var doc = new CommandDoc(this.Configuration.CommandPrefix, command);
            builder.AddField(doc.Signature, doc.Summary);
        }

        await this.ReplyAsync("Here's a list of commands:", false, builder.Build());
    }

    [Command("version")]
    [Summary("What Strive patch is the bot up-to-date with?")]
    public async Task VersionAsync() => await this.ReplyAsync("not done yet ;)");

    [Command("characters")]
    [Summary("Display Characters and their Aliases")]
    public async Task CharactersAsync()
    {
        var builder = new EmbedBuilder();

        foreach (var character in this.CharacterService.GetCharacterAliasLookup())
        {
            builder.AddField(character.Key, string.Join(", ", character));
        }

        await this.ReplyAsync("Characters can be referenced by their full names (e.g \"anji mito\") or any of these aliases:", false, builder.Build());
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

            this.Signature = $"{names.First()} {parameters}";

            this.Summary = string.IsNullOrWhiteSpace(altNames)
                ? summaryText
                : $"Alternatively: {altNames}\n{summaryText}";
        }
    }
}
