using System.Collections.Generic;

namespace StriveBot.Models.Characters
{
    public class Faust : Character
    {
        public override string FullName => "Faust";

        public override IEnumerable<string> Names => new[] { "fa", "doctor" };
    }
}