using System.Collections.Generic;

namespace StriveBot.Models.Characters
{
    public class Ky : Character
    {
        public override string FullName => "Ky Kiske";

        public override IEnumerable<string> Names => new[] { "ky", "ky-kiske" };
    }
}