using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using StriveBot.Characters;

namespace StriveBot.Services
{
    public class CharacterService
    {
        private Dictionary<string, string> NameMap { get; set; }
        private Dictionary<string, Character> InstanceMap { get; set; }

        public CharacterService()
        {
            NameMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            InstanceMap = new Dictionary<string, Character>(StringComparer.OrdinalIgnoreCase);

            foreach (Type type in Assembly.GetAssembly(typeof(Character)).GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Character))
                    && !type.IsAbstract
                    && type.IsClass))
            {
                var character = (Character)Activator.CreateInstance(type);

                foreach (var name in character.Names)
                {
                    NameMap.Add(name, character.FullName);
                }

                InstanceMap.Add(character.FullName, character);
            }
        }

        public Character ParseName(string name)
        {
            if (InstanceMap.TryGetValue(name, out var character))
            {
                return character;
            }

            if (NameMap.TryGetValue(name, out var fullName))
            {
                return InstanceMap[fullName];
            }
            else
            {
                return null;
            }
        }

        public ILookup<string, string> GetCharacterAliasLookup()
        {
            return NameMap
                .ToLookup(g => g.Value, g => g.Key);
        }
    }
}