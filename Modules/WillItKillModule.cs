namespace StriveBot.Modules;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;

using StriveBot.Models.WillItKill;
using StriveBot.Services;

[Group("will-it-kill", "Commands for managing a game of Will It Kill")]
public class WillItKillModule : InteractionModuleBase<SocketInteractionContext>
{
    public PersistenceService PersistenceService { get; set; } = default!;
    public WillItKillSession InProgress => this.PersistenceService.WillItKillSession!;

    [SlashCommand("new", "Begin a new game of Will It Kill")]
    public async Task NewGameCommandAsync() => await this.Context.Interaction.RespondWithModalAsync<NewGameForm>("newgame_modal");

    [SlashCommand("results", "Display current standings")]
    public async Task ResultsCommandAsync()
    {
        var scores = this.InProgress.Rounds
            .SelectMany(round => round.Responses
                .Select(response => new Tuple<string, bool>(response.Key, response.Value == round.Answer)))
            .GroupBy(x => x.Item1, x => x.Item2, (key, g) => new
            {
                Name = key,
                Score = g.Count(isCorrect => isCorrect),
                Total = g.Count()
            })
            .OrderByDescending(score => score.Score)
            .ThenBy(score => score.Total);

        var embed = new EmbedBuilder()
            .WithTitle("Leaderboard")
            .WithDescription(string.Join("\n\n", scores.Select(score => $"{score.Name}: {score.Score}/{score.Total}")))
            .Build();
        await this.RespondAsync(embed: embed);
    }

    [ModalInteraction("newgame_modal", ignoreGroupNames: true)]
    public async Task AfterNewGameModalAsync(NewGameForm form)
    {
        await this.RespondAsync(":thumbsup:", ephemeral: true);

        this.PersistenceService.WillItKillSession = new WillItKillSession(form.ViewingUrl, form.RoundTimestamps);

        await this.Context.Channel.SendMessageAsync(embed: this.InProgress.AsEmbed(), components: this.InProgress.AsComponent());
    }

    public class NewGameForm : IModal
    {
        public string Title => "Lets Play Will It Kill!";

        [ModalTextInput("url")]
        [InputLabel("Viewing Url")]
        public string ViewingUrl { get; set; } = string.Empty;

        [ModalTextInput("round_timestamps", TextInputStyle.Paragraph)]
        [RequiredInput(false)]
        [InputLabel("Round Timestamps (Optional)")]
        public string? RoundTimestamps { get; set; }
    }


    [ComponentInteraction("editround", ignoreGroupNames: true)]
    public async Task EditRoundButtonAsync()
    {
        var round = this.InProgress.CurrentRound;
        var modal = new ModalBuilder()
            .WithCustomId("editround_modal")
            .WithTitle($"Editing Round #{round.RoundNumber}")
            .AddTextInput("Description", "description", value: round.Description)
            .AddTextInput("Headline", "headline", value: round.Headline)
            .AddTextInput("Choice 1", "choice1", value: round.Choices[0])
            .AddTextInput("Choice 2", "choice2", value: round.Choices[1]);

        await this.Context.Interaction.RespondWithModalAsync(modal.Build());
    }

    [ModalInteraction("editround_modal", ignoreGroupNames: true)]
    public async Task AfterEditRoundModalAsync(EditRoundForm form)
    {
        await this.DeferAsync();

        var round = this.InProgress.CurrentRound;
        round.Description = form.Description;
        round.Headline = form.Headline;
        round.Choices = new[] { form.Choice1, form.Choice2 };

        await this.ModifyOriginalResponseAsync(this.FromInProgress());
    }

    public class EditRoundForm : IModal
    {
        public string Title => "Edit Round";

        [ModalTextInput("description")]
        public string? Description { get; set; }

        [ModalTextInput("headline")]
        public string Headline { get; set; } = string.Empty;

        [ModalTextInput("choice1")]
        public string Choice1 { get; set; } = string.Empty;

        [ModalTextInput("choice2")]
        public string Choice2 { get; set; } = string.Empty;
    }


    [ComponentInteraction("vote:*", ignoreGroupNames: true)]
    public async Task VoteAsync(string index)
    {
        var round = this.InProgress.CurrentRound;
        if (!round.Closed
            && int.TryParse(index, out var vote)
            && vote >= 0
            && vote < round.Choices.Length)
        {
            round.SetVote(this.Context.User.Username, vote);
        }

        await this.UpdateCurrentRoundMessage();
    }


    [ComponentInteraction("closeround", ignoreGroupNames: true)]
    public async Task CloseRoundAsync()
    {
        this.InProgress.CurrentRound.Closed = true;

        await this.UpdateCurrentRoundMessage();
    }


    [ComponentInteraction("setroundanswer", ignoreGroupNames: true)]
    public async Task SetRoundAnswerAsync(string[] data)
    {
        var round = this.InProgress.CurrentRound;
        if (round.Closed
            && int.TryParse(data.SingleOrDefault(), out var answer)
            && answer >= 0
            && answer < round.Choices.Length)
        {
            round.Answer = answer;
        }

        await this.UpdateCurrentRoundMessage();
    }


    [ComponentInteraction("nextround", ignoreGroupNames: true)]
    public async Task NextRoundAsync()
    {
        this.InProgress.NextRound();

        await this.UpdateCurrentRoundMessage();
    }


    private async Task UpdateCurrentRoundMessage() => await (this.Context.Interaction as SocketMessageComponent)!.UpdateAsync(this.FromInProgress());

    private Action<MessageProperties> FromInProgress()
    {
        return (MessageProperties msg) =>
        {
            msg.Embed = this.InProgress.AsEmbed();
            msg.Components = this.InProgress.AsComponent();
        };
    }
}

