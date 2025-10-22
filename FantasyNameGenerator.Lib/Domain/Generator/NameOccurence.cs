namespace FantasyNameGenerator.Lib.Domain.Generator
{
    public class NameOccurrence
    {
        public Dictionary<char, int> Continuations { get; set; } = [];
        public int TotalWeight { get; set; } = 0;

        public char EndChar { get; set; }

        public void AddContinuation(char continuation)
        {
            if (Continuations.TryGetValue(continuation, out int count))
                Continuations[continuation] = count + 1;
            else
                Continuations.Add(continuation, 1);
            TotalWeight++;
        }

        public char GetRandomContinuation()
        {
            if (Continuations.Count == 0)
                throw new InvalidOperationException("No continuations available.");
            if (Continuations.Count == 1)
                return Continuations.First().Key;

            int randomValue = Random.Shared.Next(TotalWeight);
            int cumulativeWeight = 0;
            foreach (var (c, weight) in Continuations)
            {
                cumulativeWeight += weight;
                if (randomValue < cumulativeWeight)
                {
                    return c;
                }
            }
            throw new InvalidOperationException("Failed to select a continuation.");
        }

        public char GetEndingContinuation()
        {
            var ending = Continuations.SingleOrDefault(c => c.Key == EndChar);
            if (ending.Value == 0)
                return GetRandomContinuation();
            return EndChar;
        }

        public char GetNonEndingContinuation()
        {
            var continuations = Continuations.Where(c => c.Key != EndChar).ToList();
            if (continuations.Count == 0)
                return GetRandomContinuation();
            if (continuations.Count == 1)
                return continuations.First().Key;

            var totalWeight = continuations.Sum(c => c.Value);
            int randomValue = Random.Shared.Next(totalWeight);
            int cumulativeWeight = 0;
            foreach (var (c, weight) in continuations)
            {
                cumulativeWeight += weight;
                if (randomValue < cumulativeWeight)
                {
                    return c;
                }
            }
            throw new InvalidOperationException("Failed to select a continuation.");
        }

        public override string ToString()
        {
            return "[" + string.Join(", ", Continuations.Select(kvp => $"{kvp.Key}: {kvp.Value}")) + "]";
        }
    }
}
