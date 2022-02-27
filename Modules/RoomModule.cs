namespace StriveBot.Modules;

using Discord;
using Discord.Commands;

using StriveBot.Services;

[Group("room")]
[Alias("roomcode")]
[Summary("Set and display room codes.")]
public class RoomModule : BaseStriveBotModule
{
    public PersistenceService PersistenceService { get; set; } = default!;

    [Command]
    [Summary("Displays the current room code.")]
    public async Task GetRoomCodeAsync() => await this.ReplyAsync(this.PersistenceService.RoomCode ?? "Room code is not set");

    [Command]
    [Priority(0)]
    [Summary("Assigns the current room code.")]
    public async Task SetRoomCodeAsync(string roomCode)
    {
        if (!IsValidRoomCode(roomCode))
        {
            await this.Context.Message.AddReactionAsync(new Emoji("❌"));
            return;
        }

        this.PersistenceService.RoomCode = roomCode;
        await this.Context.Message.AddReactionAsync(new Emoji("👀"));
        await this.Context.Client.SetGameAsync($"in room {roomCode}");
    }

    [Command("clear")]
    [Alias("leave", "disband", "x")]
    [Priority(1)]
    [Summary("Clears the current room code.")]
    public async Task ClearRoomCodeAsync()
    {
        this.PersistenceService.RoomCode = null;
        await this.Context.Client.SetGameAsync(null);
    }

    private static bool IsValidRoomCode(string roomCode)
    {
        // Keep basic for now.
        return !string.IsNullOrWhiteSpace(roomCode)
            && roomCode.Length == 6;
    }
}
