using FantasyNameGenerator.Lib.Distribution;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace FantasyNameGenerator.Lib.Generator
{
    public class MarkovGenerator : INameGenerator
    {
        public string BeginWith { get; set; } = "";
        public bool ControlLength { get; set; } = true;
        public double LengthModifier { get; set; } = 1.0;

        private readonly Dictionary<string, NameOccurrence> _occurrences = [];
        private readonly IDistribution _distribution;
        private readonly int _sequenceSize;

        const char StartChar = '^';
        const char EndChar = '$';
        const char ConnectorChar = '-';

        public MarkovGenerator(string[] names, int sequenceSize)
        {
            _sequenceSize = sequenceSize;
            _occurrences = GetOccurrences(names, _sequenceSize);
            _distribution = GetDistribution(names);
        }

        public string GenerateName()
        {
            var desiredLength = (int)(_distribution.GetRandomNumber() * LengthModifier);
            var builder = new StringBuilder();

            string startSequence;
            if (string.IsNullOrEmpty(BeginWith) || BeginWith.Length < _sequenceSize)
            {
                startSequence = GetRandomOccurrence(o => o.Key.StartsWith(StartChar + BeginWith));
            }
            else
            {
                startSequence = StartChar + BeginWith;
            }
            int currentLength = startSequence.Length - 1;
            builder.Append(startSequence);

            while (true)
            {
                char continuation = GetContinuation(startSequence, currentLength, desiredLength);
                startSequence = startSequence[1..] + continuation;
                currentLength++;
                builder.Append(continuation);

                if (continuation == EndChar)
                {
                    break;
                }
            }

            return builder.ToString().Trim(StartChar, EndChar);
        }

        private char GetContinuation(string text, int currentLength, int expectedLength)
        {
            text = text[Math.Max(0, text.Length - _sequenceSize)..];
            if (_occurrences.TryGetValue(text, out var occurrence))
            {
                if (ControlLength)
                {
                    if (currentLength >= expectedLength)
                    {
                        return occurrence.GetEndingContinuation();
                    }
                    else
                    {
                        return occurrence.GetNonEndingContinuation();
                    }
                }

                return occurrence.GetRandomContinuation();
            }

            string? closestMatch = _occurrences.Keys
                .OrderBy(key => ComputeLevenshteinDistance(text, key))
                .FirstOrDefault();
            if (closestMatch != null)
            {
                return _occurrences[closestMatch].GetRandomContinuation();
            }

            return _occurrences.ElementAt(Random.Shared.Next(_occurrences.Count)).Value.GetRandomContinuation();
        }

        private int ComputeLevenshteinDistance(string a, string b)
        {
            int[,] dp = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
                for (int j = 0; j <= b.Length; j++)
                {
                    if (i == 0)
                        dp[i, j] = j;
                    else if (j == 0)
                        dp[i, j] = i;
                    else
                        dp[i, j] = Math.Min(dp[i - 1, j] + 1, Math.Min(dp[i, j - 1] + 1,
                            dp[i - 1, j - 1] + (a[i - 1] == b[j - 1] ? 0 : 1)));
                }

            return dp[a.Length, b.Length];
        }

        private string GetRandomOccurrence(Func<KeyValuePair<string, NameOccurrence>, bool> selector)
        {
            var collection = _occurrences.Where(selector);
            if (collection.Count() == 1)
                return collection.First().Key;

            int totalWeight = collection.Sum(o => o.Value.TotalWeight);
            int randomValue = Random.Shared.Next(totalWeight);
            int cumulativeWeight = 0;
            foreach (var occurrence in collection)
            {
                cumulativeWeight += occurrence.Value.TotalWeight;
                if (randomValue < cumulativeWeight)
                    return occurrence.Key;
            }
            throw new InvalidOperationException("Failed to select a random occurrence.");
        }

        private static Dictionary<string, NameOccurrence> GetOccurrences(string[] names, int sequenceSize)
        {
            var occurrences = new Dictionary<string, NameOccurrence>();
            foreach (var name in names)
            {
                var normalizedName = NormalizeName(name);
                for (int i = sequenceSize; i < normalizedName.Length; i++)
                {
                    string key = normalizedName.Substring(i - sequenceSize, sequenceSize);
                    char continuation = normalizedName[i];
                    if (!occurrences.TryGetValue(key, out var occurrence))
                    {
                        occurrence = new NameOccurrence() { EndChar = EndChar };
                        occurrences.Add(key, occurrence);
                    }
                    occurrence.AddContinuation(continuation);
                }
            }
            return occurrences;
        }

        private static string NormalizeName(string name)
        {
            string result = name;
            if (result.StartsWith(ConnectorChar))
                result = result[1..];
            else
                result = StartChar + result;

            if (result.EndsWith(ConnectorChar))
                result = result[..^1];
            else
                result = result + EndChar;
            return result;
        }

        private static NormalDistribution GetDistribution(string[] names)
        {
            return new NormalDistribution([.. names.Select(n => n.Length)]);
        }
    }
}
