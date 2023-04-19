using Microsoft.Extensions.DependencyInjection;
using Nice.Dotnet.Wpf.ViewModels;
using Nice.Dotnet.Core;
using Nice.Dotnet.Wpf.Views;

namespace Nice.Dotnet.Wpf;

internal static class ServicesRegister
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddNiceClientService();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<ChartsWindow>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<ChartsViewModel>();

        return services;
    }
}
