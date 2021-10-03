using Discord;
using Discord.Commands;
using System.Threading.Tasks;

using StriveBot.Services;

namespace StriveBot.Modules
{
    [Group("room")]
    [Alias("roomcode")]
    public class RoomModule : BaseStriveBotModule
    {
        public PersistenceService PersistenceService { get; set; }

        [Command]
        [Summary("Displays the current room code.")]
        public async Task GetRoomCodeAsync()
        {
            await ReplyAsync(PersistenceService.RoomCode ?? "Room code is not set");
        }

        [Command]
        [Priority(0)]
        [Summary("Assigns the current room code.")]
        public async Task SetRoomCodeAsync(string roomCode)
        {
            if (!IsValidRoomCode(roomCode))
            {
                await Context.Message.AddReactionAsync(new Emoji("❌"));
                return;
            }

            PersistenceService.RoomCode = roomCode;
            await Context.Message.AddReactionAsync(new Emoji("👀"));
            await Context.Client.SetGameAsync($"in room {roomCode}");
        }

        [Command("clear")]
        [Alias("leave", "disband", "x")]
        [Priority(1)]
        [Summary("Clears the current room code.")]
        public async Task ClearRoomCodeAsync()
        {
            PersistenceService.RoomCode = null;
            await Context.Client.SetGameAsync(null);
        }

        private bool IsValidRoomCode(string roomCode)
        {
            // Keep basic for now.
            return !string.IsNullOrWhiteSpace(roomCode)
                && roomCode.Length == 6;
        }
    }
}