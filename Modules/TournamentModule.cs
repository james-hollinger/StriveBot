namespace StriveBot.Modules;

using Discord.Commands;

[Summary("Info on upcoming tournaments.")]
public class TournamentModule : BaseStriveBotModule
{
    [Command("tournaments")]
    [Summary("Displays upcoming strive tournaments.")]
    public async Task GetTournamentsAsync() => await this.ReplyWithEmbeddedUrlAsync("Upcoming Strive tournaments on Smash.gg", "https://smash.gg/tournaments?per_page=30&filter=%7B%22upcoming%22%3Atrue%2C%22videogameIds%22%3A33945%2C%22attendeeCount%22%3A%5B%22gt%3A100%2Clte%3A200%22%2C%22gt%3A200%2Clte%3A500%22%2C%22gt%3A500%2Clte%3A1000%22%2C%22gt%3A1000%22%5D%2C%22past%22%3Afalse%7D&page=1");
}
