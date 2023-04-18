using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
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
    private readonly ObservableCollection<ObservablePoint> _observablePoints;
    public ChartsViewModel()
    {
        var values = Enumerable.Range(-90,200).OrderBy(q=>Guid.NewGuid()).ToArray();
        _observablePoints = new();

        foreach (var item in values)
        {
            _observablePoints.Add(new ObservablePoint(_key++,item));

        }

        seriesCollection = new()
        {
            new StepLineSeries<ObservablePoint>
            {
                Values = _observablePoints
            }
        };
    }
    [ObservableProperty]
    ObservableCollection<ISeries> seriesCollection;
       

}
