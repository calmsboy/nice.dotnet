using CommunityToolkit.Mvvm.ComponentModel;
using Nice.Dotnet.Domain.Std.IEntities;

namespace Nice.Dotnet.Core.Models
{
    /// <summary>
    /// 用户信息可观测对象
    /// </summary>
    public partial class ObservableCustomInfo : ObservableObject, ICustomInfoRoot
    {
        [ObservableProperty]
        string name ;
        [ObservableProperty]
        string city ;
        [ObservableProperty]
        string id ;
    }
}
