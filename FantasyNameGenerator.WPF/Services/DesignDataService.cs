using FantasyNameGenerator.WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNameGenerator.WPF.Services
{
    internal class DesignDataService : IDataService
    {
        public ObservableCollection<NameCategory> GetCategories()
        {
            return
            [
                new NameCategory("Fantasy")
                {
                    Cultures =
                    [
                        new NameCulture("Elves") { Description = "The Elves." },
                        new NameCulture("Dwarves") { Description = "The Dwarves." },
                        new NameCulture("Orcs") { Description = "The Orcs." },
                    ]
                },
                new NameCategory("Historical")
                {
                    Cultures =
                    [
                        new NameCulture("Roman"),
                        new NameCulture("Greek"),
                        new NameCulture("Persian"),
                    ]
                }
            ];
        }
    }
}
