namespace Gui.Generator
{
    using System;

    /// <summary>
    /// The statistical distribution.
    /// </summary>
    internal interface IDistribution
    {
        /// <summary>
        /// Gets a random number using the distribution.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <returns>The <see cref="Double"/>.</returns>
        double GetRandomNumber(Random random);
    }
}
