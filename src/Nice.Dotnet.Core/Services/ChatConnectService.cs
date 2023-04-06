using Microsoft.AspNetCore.SignalR.Client;
using Nice.Common.Tool.IdCreate;

namespace Nice.Dotnet.Core.Services
{
    public class ChatConnectService
    {
        private HubConnection _hubConnection;
        public static string UserId { get; set; } = "";
        private string _baseUrl;
        public ChatConnectService(string baseUrl = "https://localhost:5188")
        {
            _baseUrl=baseUrl;
        }
        public async ValueTask BuildConnection(string clientType, HttpMessageHandler? messageHandler = null)
        {
            UserId = $"{clientType}:{IdCreater.GetBanksId()}";
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{_baseUrl}/api/chat", option =>
                {
                    option.HttpMessageHandlerFactory = (handler) =>
                    {
                        return messageHandler is not null? messageHandler : handler;
                    };
                })
                .Build();
            _hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(360);
            await Connect();
            Register();

        }
        public async ValueTask Connect()
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public void Register()
        {
            _hubConnection.InvokeAsync("Login",UserId);
        }

        public void SendMessage(string message) 
        { 
            _hubConnection.InvokeAsync("SendMessage",UserId, message);
        }

        public void Watch(Action<int> onlineUserCountWatch,Action<string,string> messageWatch)
        {
            _hubConnection.On<int>("OnlineUserCount",userCount =>
            {
                onlineUserCountWatch.Invoke(userCount);
            });
            _hubConnection.On<string,string>("ReceiveMessage", (user,msg) =>
            {
                messageWatch.Invoke(user, msg);
            });
        }
    }
}
