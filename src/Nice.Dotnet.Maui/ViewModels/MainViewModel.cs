using CommunityToolkit.Mvvm.Input;
using Nice.Dotnet.Core.IServices;
using Nice.Dotnet.Core.Services;
using Nice.Dotnet.Core.ViewModels;
using Nice.Dotnet.Maui.Services;

namespace Nice.Dotnet.Maui.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        public MainViewModel(ICustomInfoClientService customInfoService, IConnectivity connectivity,

            ChatConnectService chatConnectService) : base(customInfoService)
        {
            this.connectivity = connectivity;
            _chatConnectService = chatConnectService;
            var hander = new HttpsClientHandlerFactory();
            _chatConnectService.BuildConnection("MAUI", hander.GetPlatformMessageHandler()).GetAwaiter();
            _chatConnectService.Watch((online) => { }, (user, msg) =>
            {
                MessageCollect.Add($"{user}\n 发送消息：{msg} \n {DateTime.Now.ToLocalTime()}");
            });
        }

        IConnectivity connectivity;
        private readonly ChatConnectService _chatConnectService;

        [RelayCommand]
        async Task SendMessage()
        {
            _chatConnectService.SendMessage(Message);
        }

    }
}
