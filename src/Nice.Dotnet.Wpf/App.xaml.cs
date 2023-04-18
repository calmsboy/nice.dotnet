using CommunityToolkit.Mvvm.DependencyInjection;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.DependencyInjection;
using Nice.Dotnet.Core;
using Nice.Dotnet.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Nice.Dotnet.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool _initialized;
        public App()
        {

            InitializeComponent();
            if (!_initialized)
            {
                var services = new ServiceCollection();
                services.AddNiceClientService();
                services.AddTransient<MainViewModel>();
                services.AddTransient<ChartsViewModel>();
                Ioc.Default.ConfigureServices(services.BuildServiceProvider());
                _initialized = true;
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LiveCharts.Configure(config =>
                config
                    // registers SkiaSharp as the library backend
                    // REQUIRED unless you build your own
                    .AddSkiaSharp()

                    // adds the default supported types
                    // OPTIONAL but highly recommend
                    .AddDefaultMappers()

                    // select a theme, default is Light
                    // OPTIONAL
                    //.AddDarkTheme()
                    .AddLightTheme()

                );
        }
    }
}
