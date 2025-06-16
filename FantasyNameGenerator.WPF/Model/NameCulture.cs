using FantasyNameGenerator.Lib;
using FantasyNameGenerator.Lib.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FantasyNameGenerator.WPF.Model
{
    internal class NameCulture(string name)
    {
        public string Name { get; set; } = name;
        public string? Description { get; set; }

        public string? DirectoryPath { get; set; }
        public CultureMetadataJson? Metadata { get; set; }
    }
}
