using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNameGenerator.Lib.Domain.Services
{
    public interface IRandomProvider
    {
        double NextDouble();
    }
}
