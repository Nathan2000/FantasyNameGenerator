namespace FantasyNameGenerator.Lib.Distribution
{
    public class NormalDistribution : IDistribution
    {
        private static bool _generate;
        private static double _z0;
        private static double _z1;
        private readonly double _mean;
        private readonly double _deviation;

        public NormalDistribution(double mean, double deviation)
        {
            _mean = mean;
            _deviation = deviation;
        }

        public NormalDistribution(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                throw new ArgumentException("Numbers cannot be null or empty.", nameof(numbers));

            _mean = numbers.Average();
            _deviation = Math.Sqrt(numbers.Sum(n => Math.Pow(n - _mean, 2)) / (numbers.Length - 1));
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
            {
                return ScaleToMean(_z1);
            }

            double u1 = Random.Shared.NextDouble();
            double u2 = Random.Shared.NextDouble();
            _z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(TwoPi * u2);
            _z1 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(TwoPi * u2);
            return ScaleToMean(_z0);
        }

        private double ScaleToMean(double number)
        {
            return number * _deviation + _mean;
        }
    }
}
