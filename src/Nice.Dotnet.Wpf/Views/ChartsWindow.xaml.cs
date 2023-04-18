using CommunityToolkit.Mvvm.DependencyInjection;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.WPF;
using Nice.Dotnet.Wpf.ViewModels;
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
            this.DataContext= Ioc.Default.GetService<ChartsViewModel>();
        }
        private void SaveImageFromChart()
        {
            var lineChart = (CartesianChart)FindName("LineChart");
            var skLineChart = new SKCartesianChart(lineChart) { Width = 1920,Height = 1080};
            try
            {

                skLineChart.SaveImage("LineChart.png");
                MessageBox.Show("已保存在相对目录下");
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
    }
}
