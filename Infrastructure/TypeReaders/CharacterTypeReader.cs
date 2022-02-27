namespace StriveBot.Infrastructure.TypeReaders;

using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

using StriveBot.Services;

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
