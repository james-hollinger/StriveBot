using Discord;
using Discord.Commands;
using StriveBot.Characters;
using System;
using System.Threading.Tasks;

namespace StriveBot.TypeReaders
{
    public class CharacterTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var character = Character.Parse(input);
            if (character != null)
            {
                return Task.FromResult(TypeReaderResult.FromSuccess(character));
            }

            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, $"Could not recognize character {input}."));
        }
    }
}