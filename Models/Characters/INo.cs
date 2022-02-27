using System.Collections.Generic;

namespace StriveBot.Models.Characters
{
    public class INo : Character
    {
        public override string FullName => "I-No";

        public override IEnumerable<string> Names => new[] { "in", "ino", "witch" };
    }
}