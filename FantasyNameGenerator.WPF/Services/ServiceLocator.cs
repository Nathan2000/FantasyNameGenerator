using FantasyNameGenerator.Lib.Domain;
using FantasyNameGenerator.Lib.Infrastructure;
using System.ComponentModel;
using System.Windows;

namespace FantasyNameGenerator.WPF.Services
{
    internal static class ServiceLocator
    {
        public static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        // TODO: Implement separate design-time services
        public static IDataLoader DataLoader => IsInDesignMode
            ? new JsonDataLoader()
            : new JsonDataLoader();

        public static IDirectoryLoader DirectoryLoader => IsInDesignMode
            ? new FileSystemDirectoryLoader()
            : new FileSystemDirectoryLoader();

        public static INameCultureProvider NameCultureProvider => IsInDesignMode
            ? new DesignTimeNameCultureProvider()
            : new JsonNameCultureProvider("Data", DataLoader, DirectoryLoader);
    }
}
