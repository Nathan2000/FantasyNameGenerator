using CommunityToolkit.Mvvm.DependencyInjection;
using FantasyNameGenerator.WPF.Services;
using FantasyNameGenerator.WPF.ViewModels.Main;
using FantasyNameGenerator.WPF.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FantasyNameGenerator.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddApplicationServices()
                .BuildServiceProvider());
            
            var vm = Ioc.Default.GetRequiredService<IMainViewModel>();
            var mainWindow = new MainWindow()
            {
                DataContext = vm
            };
            mainWindow.Show();
        }
    }

}
