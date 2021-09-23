using System.Collections.Generic;

namespace StriveBot.Characters
{
    public abstract class Character
    {
        public abstract string FullName { get; }
        public abstract IEnumerable<string> Names { get; }
    }
}