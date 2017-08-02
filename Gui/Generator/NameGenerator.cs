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
            this.nameDistribution = this.GetDistribution(names);
        }

        /// <summary>
        /// Generates a new name.
        /// </summary>
        /// <returns>The <see cref="String"/>.</returns>
        public string GenerateName()
        {
            var expectedLength = (int)(this.nameDistribution.GetRandomNumber(Random) * this.settings.LengthModifier);
            var builder = new StringBuilder();
            string randomSequence;
            int randomNumber;

            if (string.IsNullOrEmpty(this.settings.BeginWith))
            {
                var startingSequences = this.GetCumulativeDistribution(this.occurences.Where(p => p.Key.StartsWith("^")));
                randomNumber = Random.Next(startingSequences.Last().Value);
                randomSequence = startingSequences.First(p => p.Value >= randomNumber).Key;
            }
            else
            {
                randomSequence = "^" + this.settings.BeginWith;
            }

            builder.Append(randomSequence);

            var endGeneration = randomSequence.EndsWith("$");
            for (var i = 0; i >= 100 || !endGeneration; ++i)
            {
                var seed = randomSequence.Substring(1);
                var sequences = this.GetStringsThatStartWith(this.occurences, seed);

                if (this.settings.ControlLength)
                {
                    sequences = this.ControlLength(i + this.settings.SequenceSize - 1, expectedLength, sequences);
                }

                var cumulativeDist = this.GetCumulativeDistribution(sequences);
                randomNumber = Random.Next(cumulativeDist.Last().Value + 1);
                randomSequence = cumulativeDist.First(p => p.Value >= randomNumber).Key;
                builder.Append(
                    randomSequence.Substring(
                        !string.IsNullOrEmpty(this.settings.BeginWith) && i == 0
                            ? this.settings.BeginWith.Length
                            : this.settings.SequenceSize - 1));

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
        private Dictionary<string, int> ControlLength(int currentLength, int expectedLength, Dictionary<string, int> sequences)
        {
            if (currentLength >= expectedLength)
            {
                // The name is longer than required. End it as soon as possible.
                if (sequences.Any(s => s.Key.EndsWith("$")))
                {
                    sequences = sequences.Where(s => s.Key.EndsWith("$")).ToDictionary(p => p.Key, p => p.Value);
                }
            }
            else
            {
                // The name is shorter than required. Don't end it unless it's unavoidable.
                if (sequences.Any(s => !s.Key.EndsWith("$")))
                {
                    sequences = sequences.Where(s => !s.Key.EndsWith("$")).ToDictionary(p => p.Key, p => p.Value);
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
        /// Returns the distribution of name lengths.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns>The <see cref="IDistribution"/>.</returns>
        private IDistribution GetDistribution(IEnumerable<string> names)
        {
            var properNames = names.Where(n => !n.StartsWith("-") && !n.EndsWith("-")).ToList();
            var lengths = this.GetNameLengths(properNames).OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            var mean = lengths.Sum(p => p.Value * p.Key) / (double)properNames.Count;
            var variance = lengths.Sum(p => Math.Pow(p.Key - mean, 2.0) * p.Value) / properNames.Count;
            var deviation = Math.Sqrt(variance);
            return new NormalDistribution(mean, deviation);
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
                string testedWord = this.Normalize(name);
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

        /// <summary>
        /// Filters strings that begin with a given sequence. If there are no matching strings, returns the ones that start with ANYTHING.
        /// </summary>
        /// <param name="sourceStrings">The source strings.</param>
        /// <param name="startingString">The starting string.</param>
        /// <returns>The <see cref="Dictionary{String, Int}"/></returns>
        private Dictionary<string, int> GetStringsThatStartWith(
            Dictionary<string, int> sourceStrings,
            string startingString)
        {
            var result = sourceStrings.Where(p => p.Key.StartsWith(startingString));

            if (!result.Any())
            {
                // There are no strings that match. Try to use any string then.
                result = sourceStrings.Where(p => !p.Key.StartsWith("^"));
            }

            return result.ToDictionary(p => p.Key, p => p.Value);
        }

        /// <summary>
        /// Normalizes the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="String"/>.</returns>
        private string Normalize(string name)
        {
            string result = name;
            if (result.StartsWith("-"))
            {
                result = result.Substring(1);
            }
            else
            {
                result = "^" + result;
            }

            if (result.EndsWith("-"))
            {
                result = result.Substring(0, result.Length - 1);
            }
            else
            {
                result = result + "$";
            }

            return result;
        }
    }
}
