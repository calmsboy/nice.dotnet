using CommunityToolkit.Maui;
using Nice.Dotnet.Core;
using Nice.Dotnet.Core.IServices;
using Nice.Dotnet.Maui.Services;
using Nice.Dotnet.Maui.ViewModels;

namespace Nice.Dotnet.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit();
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            var hander = new HttpsClientHandlerService();

            builder.Services.AddNiceClientService("https://10.0.2.2:5188", hander.GetPlatformMessageHandler());
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();
            return builder.Build();
        }
    }
}