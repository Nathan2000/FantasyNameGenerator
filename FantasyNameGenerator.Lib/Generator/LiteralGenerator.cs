using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNameGenerator.Lib.Generator
{
    public class LiteralGenerator : INameGenerator
    {
        private readonly string[] _names;

        public LiteralGenerator(string[] names)
        {
            _names = names;
        }

        public string GenerateName()
        {
            var random = Random.Shared.Next(_names.Length);
            return _names[random];
        }
    }
}
