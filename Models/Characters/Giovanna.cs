using System.Collections.Generic;

namespace StriveBot.Characters
{
    public class Giovanna : Character
    {
        public override string FullName => "Giovanna";

        public override IEnumerable<string> Names => new[] { "gi", "gio", "rei" };
    }
}