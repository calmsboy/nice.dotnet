using Nice.Dotnet.Core.IServices;
using Nice.Dotnet.Core.ViewModels;

namespace Nice.Dotnet.Maui.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        public MainViewModel(ICustomInfoClientService customInfoService, IConnectivity connectivity) : base(customInfoService)
        {
            this.connectivity = connectivity;
        }

        IConnectivity connectivity;



    }
}
