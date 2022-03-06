namespace StriveBot.Models.WillItKill;

public class Round
{
    public int RoundNumber { get; set; }
    public string? Description { get; set; }
    public string Headline { get; set; } = "Will It Kill?";
    public string[] Choices { get; set; } = new[] { "Yes", "No" };
    public int? Answer { get; set; }
    public bool Closed { get; set; }
    public Dictionary<string, int> Responses { get; set; } = new();

    public Round(string? description, string? headline = null, string[]? choices = null)
    {
        this.Description = description;
        this.Headline = headline ?? this.Headline;
        this.Choices = choices ?? this.Choices;
    }

    public int VoteCount(int choice) => this.Responses.Values.Count(x => x == choice);

    public void SetVote(string username, int choice) => this.Responses[username] = choice;
}
