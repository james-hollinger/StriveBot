namespace StriveBot.Services;

using StriveBot.Models.WillItKill;

public class PersistenceService
{
    public WillItKillSession? WillItKillSession { get; set; }
    public string? RoomCode { get; set; }
}
