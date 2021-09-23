using System.Collections.Generic;

namespace StriveBot.Characters
{
    public class Potemkin : Character
    {
        public override string FullName => "Potemkin";

        public override IEnumerable<string> Names => new[] { "po", "pot" };
    }
}