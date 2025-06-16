using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FantasyNameGenerator.WPF.Services
{
    internal static class ServiceLocator
    {
        public static IDataService DataService => DesignerProperties.GetIsInDesignMode(new DependencyObject())
            ? new DesignDataService()
            : new DataService();
    }
}
