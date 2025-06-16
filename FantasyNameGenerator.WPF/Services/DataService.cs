using FantasyNameGenerator.WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FantasyNameGenerator.WPF.Services
{
    internal class DataService : IDataService
    {
        const string DataFolder = "Data";
        const string MetadataFilename = "metadata.json";

        public ObservableCollection<NameCategory> GetCategories()
        {
            var result = new ObservableCollection<NameCategory>();
            var subfolders = Directory.GetDirectories(DataFolder);
            foreach (var subfolder in subfolders)
            {
                var category = new NameCategory(Path.GetFileName(subfolder))
                {
                    Cultures = GetCultures(subfolder)
                };
                result.Add(category);
            }
            return result;
        }

        private ObservableCollection<NameCulture> GetCultures(string directoryName)
        {
            var result = new ObservableCollection<NameCulture>();
            var subfolders = Directory.GetDirectories(directoryName);
            foreach (var subfolder in subfolders)
            {
                var metadataFilepath = Path.Combine(subfolder, MetadataFilename);
                if (!File.Exists(metadataFilepath))
                {
                    continue; // Skip if metadata file does not exist
                }
                var file = File.ReadAllText(metadataFilepath);
                var metadata = JsonSerializer.Deserialize<CultureMetadataJson>(file);
                if (metadata == null)
                {
                    continue;
                }

                var culture = new NameCulture(Path.GetFileName(subfolder))
                {
                    Description = metadata.Description,
                    DirectoryPath = subfolder,
                    Metadata = metadata
                };
                result.Add(culture);
            }

            return result;
        }
    }
}
