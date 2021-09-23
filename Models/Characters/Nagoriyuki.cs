using System.Collections.Generic;

namespace StriveBot.Characters
{
    public class Nagoriyuki : Character
    {
        public override string FullName => "Nagoriyuki";

        public override IEnumerable<string> Names => new[] { "na", "nago", "名残雪" };
    }
}