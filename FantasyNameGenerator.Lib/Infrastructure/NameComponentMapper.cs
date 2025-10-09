using FantasyNameGenerator.Lib.Domain.Model;
using FantasyNameGenerator.Lib.Infrastructure.DTOs;

namespace FantasyNameGenerator.Lib.Infrastructure
{
    public class NameComponentMapper(IDataLoader dataLoader)
    {
        public async Task<NameComponent> MapAsync(
            string componentName,
            NameComponentDto dto,
            string cultureDirectory,
            CancellationToken ct = default)
        {
            var maleNames = await LoadNameListAsync(cultureDirectory, dto.MaleFilename, ct);
            var femaleNames = await LoadNameListAsync(cultureDirectory, dto.FemaleFilename, ct);

            return new NameComponent(
                componentName,
                dto.Type,
                maleNames,
                femaleNames);
        }

        private async Task<IEnumerable<string>> LoadNameListAsync(string cultureDirectory, string? filename, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return [];
            string filepath = $"{cultureDirectory}/{filename}";
            var lines = await dataLoader.ReadLinesAsync(filepath, ct);
            return lines;
        }
    }
}
