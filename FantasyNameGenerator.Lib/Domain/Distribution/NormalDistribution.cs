using FantasyNameGenerator.Lib.Domain.Services;

namespace FantasyNameGenerator.Lib.Domain.Distribution
{
    public class NormalDistribution : IDistribution
    {
        private readonly IRandomProvider _random;

        public double Mean { get; }
        public double Deviation { get; }

        private bool _generate;
        private double _z0;
        private double _z1;

        public NormalDistribution(double mean, double deviation, IRandomProvider? random = null)
        {
            _random = random ?? new DefaultRandomProvider();
            Mean = mean;
            Deviation = deviation;
        }

        public NormalDistribution(int[] numbers, IRandomProvider? random = null)
        {
            if (numbers.Length == 0)
                throw new ArgumentException("Numbers cannot be empty.", nameof(numbers));

            Mean = numbers.Average();
            Deviation = Math.Sqrt(numbers.Average(n => Math.Pow(n - Mean, 2)));
            _random = random ?? new DefaultRandomProvider();
        }

        /// <summary>
        /// Generates a random number based on a normal distribution with the specified mean and standard deviation.
        /// </summary>
        /// <returns>The <see cref="double"/>.</returns>
        public double GetRandomNumber()
        {
            const double TwoPi = 2.0 * Math.PI;
            _generate = !_generate;
            if (!_generate)
                return ScaleToMean(_z1);

            double u1 = _random.NextDouble();
            double u2 = _random.NextDouble();
            _z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(TwoPi * u2);
            _z1 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(TwoPi * u2);
            return ScaleToMean(_z0);
        }

        private double ScaleToMean(double number) => number * Deviation + Mean;
    }
}
