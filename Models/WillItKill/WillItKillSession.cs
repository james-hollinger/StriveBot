namespace StriveBot.Models.WillItKill;

using System.Text.RegularExpressions;
using Discord;

public class WillItKillSession
{
    private int currentRoundindex;
    public string ViewingUrl { get; set; }
    public List<Round> Rounds { get; set; } = new();
    public Round CurrentRound => this.Rounds[this.currentRoundindex];

    public WillItKillSession(string viewingUrl, string? roundTimestamps = null) : this(viewingUrl, ParseTimestamps(roundTimestamps)) { }

    public WillItKillSession(string viewingUrl, List<Round> rounds)
    {
        this.ViewingUrl = viewingUrl;
        this.Rounds = rounds;
        this.currentRoundindex = 0;

        if (!rounds.Any())
        {
            this.AddRound(null);
        }
    }

    public void AddRound(string? description, string? headline = null, string[]? choices = null)
    {
        var question = new Round(description, headline, choices)
        {
            RoundNumber = this.Rounds.Count + 1
        };

        this.Rounds.Add(question);
    }

    public void NextRound()
    {
        this.currentRoundindex++;
        if (this.Rounds.Count == this.currentRoundindex)
        {
            this.AddRound(null);
        }
    }

    public Embed AsEmbed()
    {
        var description = string.IsNullOrWhiteSpace(this.CurrentRound.Description)
            ? ""
            : $"{this.CurrentRound.Description} : ";
        description += $"**__{this.CurrentRound.Headline}__**";

        return new EmbedBuilder()
            .WithTitle($"Round {this.CurrentRound.RoundNumber}!")
            .WithUrl(this.ViewingUrl)
            .WithDescription(description)
            .WithColor(Color.DarkPurple)
            .Build();
    }

    public MessageComponent AsComponent() => this.CurrentRound.Closed ? Closed(this.CurrentRound) : Open(this.CurrentRound);

    private static MessageComponent Open(Round round)
    {
        return new ComponentBuilder()
            .WithButton(round.Choices[0], "vote:0", ButtonStyle.Success, row: 0)
            .WithButton($"{round.VoteCount(0)}", "display1", ButtonStyle.Secondary, disabled: true, row: 0)
            .WithButton(round.Choices[1], "vote:1", ButtonStyle.Primary, row: 1)
            .WithButton($"{round.VoteCount(1)}", "display2", ButtonStyle.Secondary, disabled: true, row: 1)
            .WithButton("Edit", "editround", ButtonStyle.Secondary, row: 2)
            .WithButton("Close Voting", "closeround", ButtonStyle.Danger, row: 2)
            .Build();
    }

    private static MessageComponent Closed(Round round)
    {
        return new ComponentBuilder()
            .WithSelectMenu(new SelectMenuBuilder()
                .WithCustomId("setroundanswer")
                .WithPlaceholder("Set Final Result")
                .AddOption(round.Choices[0], "0", isDefault: round.Answer == 0)
                .AddOption(round.Choices[1], "1", isDefault: round.Answer == 1),
             row: 0)
            .WithButton("Next Round", "nextround", ButtonStyle.Danger, disabled: round.Answer == null, row: 1)
            .Build();
    }

    private static List<Round> ParseTimestamps(string? timestamps)
    {
        if (string.IsNullOrWhiteSpace(timestamps))
        {
            return new();
        }

        // M?M:SS (description) link/n
        var rx = new Regex(@"\((.*?)\)");
        return rx.Matches(timestamps)
            .Select((match, index) => new Round(match.Groups[1].Value) { RoundNumber = index + 1 })
            .ToList();
    }
}
