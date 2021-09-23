using System.Collections.Generic;

namespace StriveBot.Characters
{
    public class Sol : Character
    {
        public override string FullName => "Sol Badguy";

        public override IEnumerable<string> Names => new[] { "so", "sol-badguy", "sol", "badguy" };
    }
}