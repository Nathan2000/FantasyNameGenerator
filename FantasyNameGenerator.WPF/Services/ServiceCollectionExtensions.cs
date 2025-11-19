using FantasyNameGenerator.Lib.Domain.Services;
using FantasyNameGenerator.Lib.Infrastructure;
using FantasyNameGenerator.WPF.ViewModels.Main;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNameGenerator.WPF.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add services
            services.AddTransient<IDataLoader, JsonDataLoader>();
            services.AddTransient<IDirectoryLoader, FileSystemDirectoryLoader>();
            services.AddTransient<INameCultureProvider, JsonNameCultureProvider>(sp =>
                new JsonNameCultureProvider("Data", sp.GetRequiredService<IDataLoader>(), sp.GetRequiredService<IDirectoryLoader>()));

            // Add ViewModels
            services.AddTransient<IMainViewModel, MainViewModel>();

            return services;
        }
    }
}
