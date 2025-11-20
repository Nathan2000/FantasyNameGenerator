using FantasyNameGenerator.Lib.Domain.Distribution;
using FantasyNameGenerator.Lib.Domain.Services;
using System.Collections.Concurrent;

namespace FantasyNameGenerator.Lib.Test
{
    public class NormalDistributionTests
    {
        [Fact]
        public void Constructor_WithMeanAndDeviation_SetsProperties()
        {
            var dist = new NormalDistribution(10.0, 2.5);
            Assert.Equal(10.0, dist.Mean);
            Assert.Equal(2.5, dist.Deviation);
        }

        [Fact]
        public void Constructor_WithSampleData_ComputesMeanAndDeviation()
        {
            int[] numbers = [2, 4, 4, 4, 5, 5, 7, 9];
            var dist = new NormalDistribution(numbers);
            Assert.Equal(5.0, dist.Mean, precision: 5);
            Assert.Equal(2.0, dist.Deviation, precision: 5);
        }

        [Fact]
        public void Constructor_WithEmptyArray_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new NormalDistribution(numbers: []));
        }

        [Theory]
        [InlineData(0.0, 0.0)]
        [InlineData(100.0, 0.0)]
        public void GetRandomNumber_WithZeroDeviation_ReturnsMean(double mean, double deviation)
        {
            var dist = new NormalDistribution(mean, deviation);

            double result = dist.GetRandomNumber();
            Assert.Equal(mean, result);
        }

        [Fact]
        public void GetRandomNumber_ProducesExpectedDistribution()
        {
            const int sampleSize = 10_000;
            const double mean = 5.0;
            const double deviation = 2.0;

            const double range1 = 0.68;
            const double range2 = 0.95;
            const double range3 = 0.997;
            const double tolerance = 0.02;

            var provider = new SeededRandomProvider(42);
            var dist = new NormalDistribution(mean, deviation, provider);

            var samples = new double[sampleSize];
            for (int i = 0; i < sampleSize; i++)
            {
                samples[i] = dist.GetRandomNumber();
            }

            double sampleMean = samples.Average();
            double sampleDeviation = Math.Sqrt(samples.Average(n => Math.Pow(n - sampleMean, 2)));
            Assert.InRange(sampleMean, mean - tolerance, mean + tolerance);
            Assert.InRange(sampleDeviation, deviation - tolerance, deviation + tolerance);

            double within1Sigma = samples.Count(n => Math.Abs(n - mean) <= deviation) / (double)sampleSize;
            double within2Sigma = samples.Count(n => Math.Abs(n - mean) <= 2 * deviation) / (double)sampleSize;
            double within3Sigma = samples.Count(n => Math.Abs(n - mean) <= 3 * deviation) / (double)sampleSize;

            Assert.InRange(within1Sigma, range1 - tolerance, range1 + tolerance);
            Assert.InRange(within2Sigma, range2 - tolerance, range2 + tolerance);
            Assert.InRange(within3Sigma, range3 - tolerance, range3 + tolerance);
        }

        [Fact]
        public void GetRandomNumber_IsThreadSafe()
        {
            var provider = new SeededRandomProvider(42);
            var dist = new NormalDistribution(0.0, 1.0, provider);
            var result = new ConcurrentBag<double>();
            Parallel.For(0, 1000, _ => result.Add(dist.GetRandomNumber()));

            Assert.Equal(1000, result.Count);
        }
    }
}
