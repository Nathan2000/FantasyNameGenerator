namespace Gui.Generator
{
    using System;

    /// <summary>
    /// The random number generator using normal (Gaussian) distribution.
    /// </summary>
    internal class NormalDistribution : IDistribution
    {
        #region Static Fields

        /// <summary>
        /// The value indicating whether to generate the new pair of values.
        /// </summary>
        private static bool generate;

        /// <summary>
        /// The first generated random number.
        /// </summary>
        private static double z0;

        /// <summary>
        /// The second generated random number.
        /// </summary>
        private static double z1;

        #endregion

        #region Fields

        /// <summary>
        /// The deviation.
        /// </summary>
        private readonly double deviation;

        /// <summary>
        /// The mean.
        /// </summary>
        private readonly double mean;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalDistribution"/> class.
        /// </summary>
        /// <param name="mean">
        /// The mean.
        /// </param>
        /// <param name="deviation">
        /// The deviation.
        /// </param>
        public NormalDistribution(double mean, double deviation)
        {
            this.mean = mean;
            this.deviation = deviation;
        }

        #endregion

        /// <summary>
        /// Generates a random number using a normal distribution.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <returns>The <see cref="Double"/>.</returns>
        public double GetRandomNumber(Random random)
        {
            const double TwoPi = 2.0 * Math.PI;
            generate = !generate;
            if (!generate)
            {
                return this.ScaleToMean(z1);
            }

            var u1 = random.NextDouble();
            var u2 = random.NextDouble();
            z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(TwoPi * u2);
            z1 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(TwoPi * u2);
            return this.ScaleToMean(z0);
        }

        /// <summary>
        /// Scales a number generated with a standard normal distribution to the given mean and deviation.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>The <see cref="Double"/>.</returns>
        private double ScaleToMean(double number)
        {
            return (number * this.deviation) + this.mean;
        }
    }
}
