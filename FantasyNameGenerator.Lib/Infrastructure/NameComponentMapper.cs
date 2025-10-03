using FantasyNameGenerator.Lib.Domain;
using FantasyNameGenerator.Lib.Infrastructure.DTOs;

namespace FantasyNameGenerator.Lib.Infrastructure
{
    public class NameComponentMapper(IDataLoader dataLoader)
    {
        public async Task<NameComponent> MapAsync(
            string componentName,
            NameComponentDto dto,
            string cultureDirectory)
        {
            var maleNames = await LoadNameListAsync(cultureDirectory, dto.MaleFilename);
            var femaleNames = await LoadNameListAsync(cultureDirectory, dto.FemaleFilename);

            return new NameComponent(
                componentName,
                dto.Type,
                maleNames,
                femaleNames);
        }

        private async Task<IEnumerable<string>> LoadNameListAsync(string cultureDirectory, string? filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return [];
            string filepath = $"{cultureDirectory}/{filename}";
            var lines = await dataLoader.ReadLinesAsync(filepath);
            return lines;
        }
    }
}
