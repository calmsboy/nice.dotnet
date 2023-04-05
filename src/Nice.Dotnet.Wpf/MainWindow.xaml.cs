using CommunityToolkit.Mvvm.DependencyInjection;
using Nice.Dotnet.Wpf.ViewModels;
using System.Windows;

namespace Nice.Dotnet.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Ioc.Default.GetService<MainViewModel>();
        }

    }
}
