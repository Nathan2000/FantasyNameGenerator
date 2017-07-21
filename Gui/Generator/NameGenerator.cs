namespace Gui.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Gui.Model;

    /// <summary>
    /// The name generator.
    /// </summary>
    public class NameGenerator
    {
        private static readonly Random Random = new Random();

        private readonly Dictionary<string, int> occurences = new Dictionary<string, int>();

        private readonly NameSettings settings;

        private readonly IDistribution nameDistribution;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameGenerator"/> class.
        /// </summary>
        /// <param name="names">
        /// The names.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public NameGenerator(IEnumerable<string> names, NameSettings settings)
        {
            this.settings = settings;
            this.occurences = this.GetOccurenceDictionary(names);

            var lengths = this.GetNameLengths(names).OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            var mean = lengths.Sum(p => p.Value * p.Key) / (double)names.Count();
            var variance = lengths.Sum(p => Math.Pow(p.Key - mean, 2.0) * p.Value) / names.Count();
            var deviation = Math.Sqrt(variance);
            this.nameDistribution = new NormalDistribution(mean, deviation);
        }

        /// <summary>
        /// Generates a new name.
        /// </summary>
        /// <returns>The <see cref="String"/>.</returns>
        public string GenerateName()
        {
            var startingSequences = this.GetCumulativeDistribution(this.occurences.Where(p => p.Key.StartsWith("^")));
            var randomNumber = Random.Next(startingSequences.Last().Value);
            var randomSequence = startingSequences.First(p => p.Value >= randomNumber).Key;

            var expectedLength = (int)(this.nameDistribution.GetRandomNumber(Random) * this.settings.LengthModifier);

            var builder = new StringBuilder();
            builder.Append(randomSequence);

            var endGeneration = randomSequence.EndsWith("$");
            for (var i = 0; i >= 100 || !endGeneration; ++i)
            {
                var seed = randomSequence.Substring(1);
                var sequences = this.occurences.Where(p => p.Key.StartsWith(seed)).ToList();

                if (this.settings.ControlLength)
                {
                    sequences = this.ControlLength(i + this.settings.SequenceSize - 1, expectedLength, sequences);
                }

                var cumulativeDist = this.GetCumulativeDistribution(sequences);
                randomNumber = Random.Next(cumulativeDist.Last().Value + 1);
                randomSequence = cumulativeDist.First(p => p.Value >= randomNumber).Key;
                builder.Append(randomSequence.Substring(this.settings.SequenceSize - 1));

                if (randomSequence.EndsWith("$"))
                {
                    endGeneration = true;
                }
            }

            return builder.ToString().Trim('^', '$');
        }

        /// <summary>
        /// Limits the available options to the ones that allow to control length of the name.
        /// </summary>
        /// <param name="currentLength">The current name length.</param>
        /// <param name="expectedLength">The expected name length.</param>
        /// <param name="sequences">The sequences to filter.</param>
        /// <returns>The <see cref="List{KeyValuePair}"/>.</returns>
        private List<KeyValuePair<string, int>> ControlLength(int currentLength, int expectedLength, List<KeyValuePair<string, int>> sequences)
        {
            if (currentLength >= expectedLength)
            {
                // The name is longer than required. End it as soon as possible.
                if (sequences.Any(s => s.Key.EndsWith("$")))
                {
                    sequences = sequences.Where(s => s.Key.EndsWith("$")).ToList();
                }
            }
            else
            {
                // The name is shorter than required. Don't end it unless it's unavoidable.
                if (sequences.Any(s => !s.Key.EndsWith("$")))
                {
                    sequences = sequences.Where(s => !s.Key.EndsWith("$")).ToList();
                }
            }

            return sequences;
        }

        /// <summary>
        /// Turns the distribution dictionary into a cumulative distribution.
        /// </summary>
        /// <param name="distribution">The distribution.</param>
        /// <returns>The <see cref="Dictionary{String, Int}"/>.</returns>
        private Dictionary<string, int> GetCumulativeDistribution(IEnumerable<KeyValuePair<string, int>> distribution)
        {
            var sequenceCount = 0;
            return distribution.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => sequenceCount += p.Value);
        }

        /// <summary>
        /// Gets the numbers of occurences of each length.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns>The <see cref="Dictionary{Int, Int}"/></returns>
        private Dictionary<int, int> GetNameLengths(IEnumerable<string> names)
        {
            var result = new Dictionary<int, int>();
            foreach (var name in names)
            {
                var length = name.Length;
                if (result.ContainsKey(length))
                {
                    result[name.Length]++;
                }
                else
                {
                    result.Add(length, 1);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the occurence dictionary of short strings.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <returns>The <see cref="Dictionary{String, Int}"/></returns>
        private Dictionary<string, int> GetOccurenceDictionary(IEnumerable<string> strings)
        {
            var result = new Dictionary<string, int>();
            foreach (var name in strings)
            {
                var testedWord = "^" + name + "$";
                for (var i = 0; i < testedWord.Length - this.settings.SequenceSize + 1; ++i)
                {
                    var sequence = new string(testedWord.Skip(i).Take(this.settings.SequenceSize).ToArray());
                    if (result.ContainsKey(sequence))
                    {
                        result[sequence]++;
                    }
                    else
                    {
                        result.Add(sequence, 1);
                    }
                }
            }

            return result;
        }
    }
}
