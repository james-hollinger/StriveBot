using System.Collections.Generic;

namespace StriveBot.Models.Characters
{
    public class Ramlethal : Character
    {
        public override string FullName => "Ramlethal Valentine";

        public override IEnumerable<string> Names => new[] { "ra", "ramlethal-valentine", "ramlethal", "ram" };
    }
}