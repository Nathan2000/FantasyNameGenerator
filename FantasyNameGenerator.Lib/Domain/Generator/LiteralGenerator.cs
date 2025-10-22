namespace FantasyNameGenerator.Lib.Domain.Generator
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
