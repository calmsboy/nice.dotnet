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
            var api = "https://10.0.2.2:5188";
#if WINDOWS
            api = "https://localhost:5188";
#endif

            builder.Services.AddNiceClientService(api, hander.GetPlatformMessageHandler());
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();
            return builder.Build();
        }
    }
}