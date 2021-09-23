using System.Collections.Generic;

namespace StriveBot.Characters
{
    public class Anji : Character
    {
        public override string FullName => "Anji Mito";

        public override IEnumerable<string> Names => new[] { "an", "anji-mito", "anji" };
    }
}