using Discord.Commands;
using System.Threading.Tasks;

using StriveBot.Characters;

namespace StriveBot.Modules
{
    [Group("wiki")]
    public class WikiModule : ModuleBase<SocketCommandContext>
    {
        private static string _dustloopWiki = "https://www.dustloop.com/wiki/index.php?title=GGST";

        [Command]
        [Summary("URL of the Dustloop Wiki")]
        public async Task WikiInfoAsync()
        {
            await ReplyAsync(_dustloopWiki);
        }

        [Command]
        [Summary("URL of the Dustloop Wki page for the character")]
        public async Task CharacterInfoAsync(Character character)
        {
            var escapedName = character.FullName.Replace(" ", "_");
            await ReplyAsync($"{_dustloopWiki}/{escapedName}");
        }
    }
}