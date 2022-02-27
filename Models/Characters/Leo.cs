namespace StriveBot.Models.Characters;

public class Leo : Character
{
    public override string FullName => "Leo Whitefang";

    public override IEnumerable<string> Names => new[] { "le", "leo-whitefang", "leo" };
}
