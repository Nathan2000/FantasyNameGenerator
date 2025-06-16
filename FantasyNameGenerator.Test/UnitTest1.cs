using FantasyNameGenerator.Lib.Composer;
using FantasyNameGenerator.Lib.Generator;
using FantasyNameGenerator.Lib.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            var names = File.ReadAllLines("Data/male.txt");
            var generator = new MarkovGenerator(names, 3);
            var name = generator.GenerateName();

            Assert.Pass($"Name generated: {name.Trim('^', '$')}");
        }

        [Test]
        public void TestNameComposer()
        {
            var metadata = new CultureMetadata("Random", "", "{name}", new Dictionary<string, NameComponent>
            {
                {
                    "name",
                    new NameComponent
                    {
                        Type = ComponentType.Markov,
                        MaleNames = File.ReadAllLines("Data/male.txt"),
                        FemaleNames = File.ReadAllLines("Data/female.txt")
                    }
                }
            });
            var composer = new NameComposer(Gender.Male, 2, metadata);
            var name = composer.Generate();
            Assert.Pass($"Name composed: {name}");
        }
    }
}
