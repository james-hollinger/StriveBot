namespace StriveBot.Models.Characters;

public abstract class Character
{
    public abstract string FullName { get; }
    public abstract IEnumerable<string> Names { get; }
}
