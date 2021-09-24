using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

using StriveBot.Services;

namespace StriveBot.Infrastructure.TypeReaders
{
    public class CharacterTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var character = services.GetRequiredService<CharacterService>()
                .ParseName(input);

            if (character != null)
            {
                return Task.FromResult(TypeReaderResult.FromSuccess(character));
            }

            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, $"Could not recognize character name \"{input}\"."));
        }
    }
}