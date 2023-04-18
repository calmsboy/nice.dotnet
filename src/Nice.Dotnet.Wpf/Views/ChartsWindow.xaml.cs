using CommunityToolkit.Mvvm.DependencyInjection;
using Nice.Dotnet.Wpf.ViewModels;
using System.Windows;

namespace Nice.Dotnet.Wpf.Views
{
    /// <summary>
    /// ChartsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChartsWindow : Window
    {
        public ChartsWindow()
        {
            InitializeComponent();
            this.DataContext= Ioc.Default.GetService<ChartsViewModel>();
        }
    }
}
