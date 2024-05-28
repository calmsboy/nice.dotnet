using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MiniExcelLibs;
using Serilog;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Nice.Dotnet.Wpf.ViewModels;


public partial class ChartsViewModel: ObservableObject
{
   
    private int _key = 0;
    private int _delay = 100;
    private readonly Random _random = new Random();
    private bool _finish = false;

    private readonly ObservableCollection<ObservablePoint>? _observablePoints;
    private readonly ObservableCollection<ObservablePoint> _observablePointsTrainPip;
    private readonly ObservableCollection<ObservablePoint> _observablePointsLinder;
    private readonly ObservableCollection<ObservablePoint> _observablePointsAddLinder;
    private readonly ObservableCollection<ObservablePoint> _observablePointsStopLinder;

    private ConcurrentQueue<Test> _rowsQueue = new();
    private Action<Action> _onUIThread;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    public ChartsViewModel(Action<Action> onUIThread)
    {
        _observablePointsTrainPip = new();
        _observablePointsLinder = new();
        _observablePointsAddLinder = new();
        _observablePointsStopLinder = new();
        seriesCollection = new();
        _onUIThread= onUIThread;
    }
    
    [ObservableProperty]
    ObservableCollection<ISeries> seriesCollection;
    

    [RelayCommand]
    async Task ReadExcel()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog() { };
        bool? result= dialog.ShowDialog();
        if (result==true)
        {
            await HandlerExcel(dialog.FileName);
        }
    }
    

    async Task HandlerExcel(string fullName)
    {

        _stopwatch.Start();
        var rows = await MiniExcel.QueryAsync<Test>(fullName);
        rows = rows.Where(q => q.TrainPip <= 1000);
        //rows = rows.Take(10000);
        _rowsQueue = new ConcurrentQueue<Test>(rows);
        _stopwatch.Stop();
        Log.Information($"Excel加载[{rows.Count()}]条数据,共需时间: {_stopwatch.Elapsed}");
        _delay = 2;
        _finish = false;
        ClearData();
        //ChartViewHidden();
        SeriesCollection.Clear();

        SettingCharts();
        _stopwatch.Start();
        foreach (var item in Enumerable.Range(1,6))
        {
            _ = Task.Run(()=>
            {
                Log.Information($"当前线程号：{Thread.CurrentThread.ManagedThreadId}");
                AnalysisData();
            });
        }
       
        
    }
    public Object Sync { get; } = new object();
    /// <summary>
    /// 用于解决ObservableCollection类型---该类型的 CollectionView，
    /// 不支持从调度程序线程以外的线程对其 SourceCollection 进行的更改。
    /// </summary>
    /// <param name="action"></param>
    void HandleDataToCollection(Action action)
    {
        _ = ThreadPool.QueueUserWorkItem(delegate
        {
            SynchronizationContext
                .SetSynchronizationContext
                (
                    new DispatcherSynchronizationContext(Application.Current.Dispatcher)
                );
            SynchronizationContext.Current?.Post(PlacementMode =>
            {
                action.Invoke();
            }, null);
        });
    }
    private async void AnalysisData()
    {
        await Task.Delay(_delay);
        //lock后是消费队列有序
        lock (Sync)
        {
            while (_rowsQueue.TryDequeue(out Test? test))
            {
                //ui线程版，不然使用默认多线程会出现IEnumerable内容被更改的情况
                HandleDataToCollection(() =>
                {
                    _observablePointsTrainPip.Add(new ObservablePoint(test.Id, test.TrainPip));
                    //if(_observablePointsTrainPip.Count()>66) _observablePointsTrainPip.RemoveAt(0);
                    _observablePointsLinder.Add(new ObservablePoint(test.Id, test.Linder));
                    _observablePointsAddLinder.Add(new ObservablePoint(test.Id, test.AddLinder));
                    _observablePointsStopLinder.Add(new ObservablePoint(test.Id, test.StopLinder));
                });
                //_onUIThread.Invoke(() =>
                //{
                    
                //});
                Thread.Sleep(2);
            }
        }
       
        _stopwatch.Stop();
        Log.Information($"LiveCharts加载数据,共需时间: {_stopwatch.Elapsed}");
        ChartViewShow();
        if (!_finish)
        {
            _finish=true;
            MessageBox.Show("数据渲染完毕");
        }
       
    }
    void SettingCharts()
    {

        SeriesCollection.Add(new LineSeries<ObservablePoint>
        {
            //LineSmoothness = 1,
            //GeometrySize = 6,
            //GeometryStroke = new SolidColorPaint(s_blue, 2),
            //Stroke = new SolidColorPaint(s_blue, 2),
            //ScalesYAt = 0,
            Fill = null,
            Stroke = new SolidColorPaint(s_blue, 1),
            GeometryFill = null,
            GeometryStroke = null,
            LineSmoothness = 0,
            Name = "lC",//列车管(kPa)
            Values = _observablePointsTrainPip,
            IsVisible = true,
        });
        SeriesCollection.Add(new LineSeries<ObservablePoint>
        {
            Fill = null,
            Stroke = new SolidColorPaint(s_purple, 1),
            GeometryFill = null,
            GeometryStroke = null,
            LineSmoothness = 0,
            Name = "2号缸",//加缓缸(kPa)
            Values = _observablePointsAddLinder,
            IsVisible = true,
        });
        SeriesCollection.Add(new LineSeries<ObservablePoint>
        {
            Fill = null,
            Stroke = new SolidColorPaint(s_red, 1),
            GeometryFill = null,
            GeometryStroke = null,
            LineSmoothness = 0,
            Name = "2号缸",//副风缸(kPa)
            Values = _observablePointsLinder,
            IsVisible = true,
        });
        SeriesCollection.Add(new LineSeries<ObservablePoint>
        {
            Fill = null,
            Stroke = new SolidColorPaint(s_yellow, 1),
            GeometryFill = null,
            GeometryStroke = null,
            LineSmoothness = 0,
            Name = "4号缸",//制动缸(kPa)
            Values = _observablePointsStopLinder,
            IsVisible = true,
        });

    }
    void ClearData()
    {
        _observablePointsTrainPip.Clear();
        _observablePointsAddLinder.Clear();
        _observablePointsLinder.Clear();
        _observablePointsStopLinder.Clear();
    }
    
    
    /// <summary>
    /// 默认图表示例
    /// </summary>
    void DefaultExample()
    {
        var values = Enumerable.Range(-90, 999).OrderBy(q => Guid.NewGuid()).ToArray();
        //var values = Enumerable.Range(-990, 999999999).Select((k,index) => new {index,value=Guid.NewGuid() }).ToArray();

        foreach (var item in values)
        {
            _observablePoints.Add(new ObservablePoint(_key++, item));
        }

        SeriesCollection.Add(new LineSeries<ObservablePoint>
        {
            Values = _observablePoints
        });
    }

}
sealed class Test
{
    public long Id { get; set; }
    public int TrainPip { get; set; }
    public int Linder { get; set; }

    public int AddLinder { get; set; }
    
    public int StopLinder { get; set;}
}

