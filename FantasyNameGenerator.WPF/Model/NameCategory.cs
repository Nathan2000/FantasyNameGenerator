using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNameGenerator.WPF.Model
{
    internal class NameCategory(string name)
    {
        public string Name { get; set; } = name;
        public ObservableCollection<NameCulture> Cultures { get; set; } = [];
    }
}
