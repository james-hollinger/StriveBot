namespace StriveBot.Services;

using System.Reflection;

using StriveBot.Models.Characters;

public class CharacterService
{
    private Dictionary<string, string> NameMap { get; set; }
    private Dictionary<string, Character> InstanceMap { get; set; }

    public CharacterService()
    {
        this.NameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        this.InstanceMap = new Dictionary<string, Character>(StringComparer.OrdinalIgnoreCase);

        foreach (var type in Assembly.GetAssembly(typeof(Character))!.GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Character))
                && !type.IsAbstract
                && type.IsClass))
        {
            var character = (Character)Activator.CreateInstance(type)!;

            foreach (var name in character.Names)
            {
                this.NameMap.Add(name, character.FullName);
            }

            this.InstanceMap.Add(character.FullName, character);
        }
    }

    public Character? ParseName(string name)
    {
        if (this.InstanceMap.TryGetValue(name, out var character))
        {
            return character;
        }

        if (this.NameMap.TryGetValue(name, out var fullName))
        {
            return this.InstanceMap[fullName];
        }
        else
        {
            return null;
        }
    }

    public ILookup<string, string> GetCharacterAliasLookup()
    {
        return this.NameMap
            .ToLookup(g => g.Value, g => g.Key);
    }
}
