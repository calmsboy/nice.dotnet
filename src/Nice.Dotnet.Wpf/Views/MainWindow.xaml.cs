using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nice.Dotnet.Wpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Nice.Dotnet.Wpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = App.ServiceProvideR?.GetService<MainViewModel>();
    }

    private void ChartSwitch_Click(object sender, RoutedEventArgs e)
    {
        var chartsWindow = new ChartsWindow();
        chartsWindow?.Show();
    }

    private void Bold_Checked(object sender, RoutedEventArgs e)
    {
        textBox1.FontWeight = FontWeights.Bold;
    }

    private void Bold_Unchecked(object sender, RoutedEventArgs e)
    {
        textBox1.FontWeight = FontWeights.Normal;
    }

    private void Italic_Checked(object sender, RoutedEventArgs e)
    {
        textBox1.FontStyle = FontStyles.Italic;
    }

    private void Italic_Unchecked(object sender, RoutedEventArgs e)
    {
        textBox1.FontStyle = FontStyles.Normal;
    }

    private void IncreaseFont_Click(object sender, RoutedEventArgs e)
    {
        if (textBox1.FontSize < 18)
        {
            textBox1.FontSize += 2;
        }
    }

    private void DecreaseFont_Click(object sender, RoutedEventArgs e)
    {
        if (textBox1.FontSize > 10)
        {
            textBox1.FontSize -= 2;
        }
    }
}
