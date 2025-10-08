using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNameGenerator.Lib.Infrastructure
{
    public interface IDirectoryLoader
    {
        IEnumerable<string> GetDirectories(string path);
        bool DirectoryExists(string path);
    }
}
