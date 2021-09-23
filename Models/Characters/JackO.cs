using System.Collections.Generic;

namespace StriveBot.Characters
{
    public class JackO : Character
    {
        public override string FullName => "Jack-O";

        public override IEnumerable<string> Names => new[] { "jc", "jacko", "jack" };
    }
}