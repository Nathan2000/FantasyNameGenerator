using FantasyNameGenerator.Lib.Domain.Services;
using FantasyNameGenerator.Lib.Infrastructure;
using System.ComponentModel;
using System.Windows;

namespace FantasyNameGenerator.WPF.Services
{
    internal static class ServiceLocator
    {
        public static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        // TODO: Implement separate design-time services
        public static IDataLoader DataLoader => new JsonDataLoader();

        public static IDirectoryLoader DirectoryLoader => new FileSystemDirectoryLoader();

        public static INameCultureProvider NameCultureProvider => new JsonNameCultureProvider("Data", DataLoader, DirectoryLoader);
    }
}
