using CommunityToolkit.Mvvm.DependencyInjection;
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
                Ioc.Default.ConfigureServices(services.BuildServiceProvider());
            }
        }
    }
}
