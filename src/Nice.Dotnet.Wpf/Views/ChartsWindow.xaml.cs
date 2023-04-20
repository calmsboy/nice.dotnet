using CommunityToolkit.Mvvm.DependencyInjection;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.WPF;
using Microsoft.Extensions.DependencyInjection;
using Nice.Dotnet.Wpf.ViewModels;
using Serilog;
using SkiaSharp;
using System;
using System.IO;
using System.Windows;

namespace Nice.Dotnet.Wpf.Views;

/// <summary>
/// ChartsWindow.xaml 的交互逻辑
/// </summary>
public partial class ChartsWindow : Window
{
    private const string _fontFamliy = "Microsoft YaHei";
    public ChartsWindow()
    {
        InitializeComponent();
        var vm = new ChartsViewModel(InvokeOnUIThread);
        this.DataContext = vm;
        //this.DataContext= App.ServiceProvideR.GetService<ChartsViewModel>();
    }
    private void InvokeOnUIThread(Action action)
    {
        Dispatcher.BeginInvoke(action);
    }
    public SolidColorPaint LegendTextPaint { get; set; } = new SolidColorPaint
    {
        Color = new SKColor(50, 50, 50),
        SKTypeface = SKTypeface.FromFamilyName(_fontFamliy)
    };
    private void SaveImageFromChart()
    {
        var lineChart = (CartesianChart)FindName("LineChart");
        var skLineChart = new SKCartesianChart(lineChart) 
        { 
            Width = 1920,
            Height = 1080,
            LegendTextPaint = LegendTextPaint,
            TooltipTextPaint = LegendTextPaint
        };
        try
        {
            skLineChart.SaveImage("LineChart.png");
            MessageBox.Show("已保存在相对目录下");
            Log.Information("LineChart.png已保存在相对目录下");
        }
        catch (Exception)
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
        //e.Cancel = true;
        //Hide();
    }

}
