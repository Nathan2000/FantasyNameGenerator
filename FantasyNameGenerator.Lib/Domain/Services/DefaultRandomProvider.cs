namespace FantasyNameGenerator.Lib.Domain.Services
{
    public class DefaultRandomProvider : IRandomProvider
    {
        public double NextDouble()
        {
            return Random.Shared.NextDouble();
        }
    }
}
