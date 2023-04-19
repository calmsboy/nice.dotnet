using CommunityToolkit.Mvvm.DependencyInjection;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.WPF;
using Microsoft.Extensions.DependencyInjection;
using Nice.Dotnet.Wpf.ViewModels;
using Serilog;
using System.IO;
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
            this.DataContext= App.ServiceProvideR.GetService<ChartsViewModel>();
        }
        private void SaveImageFromChart()
        {
            var lineChart = (CartesianChart)FindName("LineChart");
            var skLineChart = new SKCartesianChart(lineChart) { Width = 1920,Height = 1080};
            try
            {

                skLineChart.SaveImage("LineChart.png");
                MessageBox.Show("已保存在相对目录下");
                Log.Information("LineChart.png已保存在相对目录下");
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveImageFromChart();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

    }
}
