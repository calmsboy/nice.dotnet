using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nice.Dotnet.Wpf.ViewModels;
using System.Windows;

namespace Nice.Dotnet.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.ServiceProvideR.GetService<MainViewModel>();
        }

        private void ChartSwitch_Click(object sender, RoutedEventArgs e)
        {
            var chartsWindow = App.ServiceProvideR.GetService<ChartsWindow>();
            chartsWindow?.Show();
        }
    }
}
