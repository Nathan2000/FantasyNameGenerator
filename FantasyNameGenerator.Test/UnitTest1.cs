using FantasyNameGenerator.Lib.Domain.Generator;

namespace FantasyNameGenerator.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestNameGenerator()
        {
            var names = File.ReadAllLines("Data/Slavic/male.txt");
            var generator = new MarkovGenerator(names, 3);
            var name = generator.GenerateName();

            Assert.Pass($"Name generated: {name.Trim('^', '$')}");
        }
    }
}
