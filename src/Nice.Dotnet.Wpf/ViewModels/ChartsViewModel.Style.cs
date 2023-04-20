using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nice.Dotnet.Wpf.ViewModels;

public partial class ChartsViewModel
{
    private const string _fontFamliy = "Microsoft YaHei";
    private static readonly SKColor s_blue = new(25, 118, 210);
    private static readonly SKColor s_red = new(229, 57, 53);
    private static readonly SKColor s_yellow = new(198, 167, 0);
    private static readonly SKColor s_purple = new(128, 0, 128);
    /// <summary>
    /// 图表可视化控制
    /// </summary>
    [ObservableProperty]
    GridLength chartViewWidth = new GridLength(1, GridUnitType.Star);
    [ObservableProperty]
    string loadText = "导入Excel数据";
    void ChartViewShow()
    {
        ChartViewWidth = new GridLength(1, GridUnitType.Star);
        LoadText = "导入Excel数据";
    }
    void ChartViewHidden()
    {
        LoadText = "加载数据当中";
        ChartViewWidth = new GridLength(0, GridUnitType.Star);
    }
    /// <summary>
    /// 列车杠数据项显示开关
    /// </summary>
    [RelayCommand]
    void TrainPipVisibility()
    {
        if (SeriesCollection.Count()>0&&SeriesCollection[0] is not null)
        {
            SeriesCollection[0].IsVisible = !SeriesCollection[0].IsVisible;
        }
        
    }
    [RelayCommand]
    void LinderVisibility()
    {
        if (SeriesCollection.Count() > 0 && SeriesCollection[1] is not null) 
            SeriesCollection[1].IsVisible = !SeriesCollection[1].IsVisible;
    }
    [RelayCommand]
    void AddLinderVisibility()
    {
        if (SeriesCollection.Count() > 0 && SeriesCollection[2] is not null) 
            SeriesCollection[2].IsVisible = !SeriesCollection[2].IsVisible;
    }
    [RelayCommand]
    void StopLinderVisibility()
    {
        if (SeriesCollection.Count() > 0 && SeriesCollection[3] is not null) 
            SeriesCollection[3].IsVisible = !SeriesCollection[3].IsVisible;
    }
    /// <summary>
    /// 用于解决中文乱码
    /// </summary>
    public SolidColorPaint LegendTextPaint { get; set; } = new SolidColorPaint
    {
        Color = new SKColor(50, 50, 50),
        SKTypeface = SKTypeface.FromFamilyName(_fontFamliy)
    };

    /// <summary>
    /// X轴标题设置
    /// </summary>
    [Obsolete("暂时不再使用该属性")]
    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Labeler = value =>
            {
               value = value>=DateTime.MinValue.Ticks&&value<=DateTime.MaxValue.Ticks
                        ?value:DateTime.MinValue.Ticks;

               return  new DateTime((long)value).ToString("MM/dd HH:mm:ss");
            },
            LabelsPaint = new SolidColorPaint
            {
                Color = new SKColor(50, 50, 50),
                SKTypeface = SKTypeface.FromFamilyName(_fontFamliy)
            },
            LabelsRotation = 140,
            UnitWidth = TimeSpan.FromSeconds(10).Ticks,
            MinStep = TimeSpan.FromSeconds(10).Ticks
        }
    };

    
}
