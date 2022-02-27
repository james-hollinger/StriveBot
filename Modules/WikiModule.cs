namespace StriveBot.Modules;

using Discord.Commands;

using StriveBot.Models.Characters;

[Group("wiki")]
[Summary("Dustloop wiki links and character information.")]
public class WikiModule : BaseStriveBotModule
{
    private const string DustloopWiki = "https://www.dustloop.com/wiki/index.php?title=GGST";

    [Command]
    [Priority(-1)]
    [Summary("URL of the Dustloop Wiki")]
    public async Task WikiInfoAsync() => await this.ReplyWithEmbeddedUrlAsync("Dustloop Wiki", DustloopWiki);

    [Command("jubei")]
    [Summary("URL of the Dustloop Wiki page for Jubei from BB:CF")]
    public async Task JubeiInfoAsync() => await this.ReplyWithEmbeddedUrlAsync("Jubei", "http://www.dustloop.com/wiki/index.php?title=BBCF/Jubei");

    [Command]
    [Summary("URL of the Dustloop Wki page for the character")]
    public async Task CharacterInfoAsync(Character character)
    {
        var escapedName = character.FullName.Replace(" ", "_");
        await this.ReplyWithEmbeddedUrlAsync(character.FullName, $"{DustloopWiki}/{escapedName}");
    }
}
