using System.Collections.Generic;

namespace StriveBot.Models.Characters
{
    public class Axl : Character
    {
        public override string FullName => "Axl Low";

        public override IEnumerable<string> Names => new[] { "ax", "axl-low", "axl" };
    }
}