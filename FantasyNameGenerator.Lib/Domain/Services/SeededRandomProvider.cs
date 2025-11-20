namespace FantasyNameGenerator.Lib.Domain.Services
{
    public class SeededRandomProvider(int seed) : IRandomProvider
    {
        private readonly Random _random = new(seed);
        public double NextDouble() => _random.NextDouble();
    }
}
