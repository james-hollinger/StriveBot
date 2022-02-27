using System.Collections.Generic;

namespace StriveBot.Models.Characters
{
    public class May : Character
    {
        public override string FullName => "May";

        public override IEnumerable<string> Names => new[] { "ma" };
    }
}