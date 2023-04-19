using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using MiniExcelLibs;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.Dotnet.Wpf.ViewModels;

[ObservableObject]
public partial class ChartsViewModel
{
    private int _key = 0;
    private int _delay = 100;
    private readonly Random _random = new Random();
    private readonly ObservableCollection<ObservablePoint> _observablePoints;
    private readonly ObservableCollection<DateTimePoint> _observablePointsTrainPip;
    private readonly ObservableCollection<DateTimePoint> _observablePointsLinder;
    private readonly ObservableCollection<DateTimePoint> _observablePointsAddLinder;
    private readonly ObservableCollection<DateTimePoint> _observablePointsStopLinder;
    private DateTimePoint _currentPoint = new DateTimePoint();
    private ConcurrentQueue<Test> _rowsQueue = new();
    public ChartsViewModel()
    {
        _observablePointsTrainPip = new();
        _observablePointsLinder = new();
        _observablePointsAddLinder = new();
        _observablePointsStopLinder = new();
        seriesCollection = new();
    }
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
        var rows = await MiniExcel.QueryAsync<Test>(fullName);
        _rowsQueue.Clear();
        foreach (var row in rows)
        {
            _rowsQueue.Enqueue(row);
        }

        _delay = 1;
        ClearData();

        SeriesCollection.Add(new LineSeries<DateTimePoint>
        {
            Values = _observablePointsTrainPip
        });
        Action ReadDataToChartsAction = async () =>
        {
            Test? test = new();
            while (_rowsQueue.TryDequeue(out test)) 
            {
                if (test is not null)
                {
                    lock (Sync)
                    {
                        _currentPoint = new DateTimePoint(test.Timestamp, test.TrainPip);
                        _observablePointsTrainPip.Add(_currentPoint);
                    }
                }
            }
        };
        _ = Task.Run(() =>
        {
            Parallel.Invoke(ReadDataToChartsAction, ReadDataToChartsAction);
        });
        //_ = Task.Run(ReadOne);
        //_ = Task.Run(() => { ReadTree(two); });
        //AnalyseData(rows);
    }
    void ClearData()
    {
        _observablePointsTrainPip.Clear();
        _observablePointsAddLinder.Clear();
        _observablePointsLinder.Clear();
        _observablePointsStopLinder.Clear();

    }
    public Object Sync { get; } = new object();
    public Object SyncOne { get; } = new object();
    public Object SyncTwo { get; } = new object();
    Task AnalyseData(IEnumerable<Test> rows)
    {
        ClearData();
       return Task.Run(() =>
        {
            foreach (var row in rows.Take(10000))
            {
                _observablePointsTrainPip.Add(new DateTimePoint(row.Timestamp, _random.Next(-99,999)+1));
                _observablePointsAddLinder.Add(new DateTimePoint(row.Timestamp, _random.Next(-99, 999) + 1));
                _observablePointsLinder.Add(new DateTimePoint(row.Timestamp, _random.Next(-99, 999) + 1));
                _observablePointsStopLinder.Add(new DateTimePoint(row.Timestamp, _random.Next(-99, 999) + 1));
                //_observablePointsTrainPip.Add(new DateTimePoint(row.Timestamp, row.TrainPip));
                //_observablePointsAddLinder.Add(new DateTimePoint(row.Timestamp, row.Linder));
                //_observablePointsLinder.Add(new DateTimePoint(row.Timestamp, row.AddLinder));
                //_observablePointsStopLinder.Add(new DateTimePoint(row.Timestamp, row.StopLinder));
            }
            SeriesCollection.Add(new LineSeries<DateTimePoint>
            {

                Values = _observablePointsTrainPip
            });
            SeriesCollection.Add(new LineSeries<DateTimePoint>
            {
                Values = _observablePointsAddLinder
            });
            SeriesCollection.Add(new LineSeries<DateTimePoint>
            {
                Values = _observablePointsLinder
            });
            SeriesCollection.Add(new LineSeries<DateTimePoint>
            {
                Values = _observablePointsStopLinder
            });
        });
    }

    

    private async void ReadTwo(IEnumerable<Test> tests)
    {
        SeriesCollection.Add(new LineSeries<DateTimePoint>
        {
            Values = _observablePointsLinder
        });
        await Task.Delay(2);
        foreach (var row in tests)
        {
            await Task.Delay(_delay);
            _currentPoint = new DateTimePoint(row.Timestamp, row.TrainPip);
            lock (SyncOne)
            {
                _observablePointsLinder.Add(_currentPoint);
            }
        }
    }
    private async void ReadTree(IEnumerable<Test> tests)
    {
        SeriesCollection.Add(new LineSeries<DateTimePoint>
        {
            Values = _observablePointsAddLinder
        });
        await Task.Delay(2);
        lock (tests)
        {
            foreach (var row in tests)
            {
                _currentPoint = new DateTimePoint(row.Timestamp, row.AddLinder);
                lock (SyncTwo)
                {
                    _observablePointsAddLinder.Add(_currentPoint);
                }

            }
        }
        
    }

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
            LabelsRotation = 40,
            UnitWidth = TimeSpan.FromSeconds(10).Ticks,
            MinStep = TimeSpan.FromSeconds(10).Ticks
        }
    };
}
public class Test
{
    public DateTime Timestamp { get; set; }
    public int TrainPip { get; set; }
    public int Linder { get; set; }

    public int AddLinder { get; set; }
    
    public int StopLinder { get; set;}
}
