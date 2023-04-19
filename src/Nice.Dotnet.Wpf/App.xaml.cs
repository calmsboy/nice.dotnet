using CommunityToolkit.Mvvm.DependencyInjection;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nice.Dotnet.Core;
using Nice.Dotnet.Extension.LoggerSupport;
using Nice.Dotnet.Wpf.ViewModels;
using Nice.Dotnet.Wpf.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Nice.Dotnet.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private bool _initialized;
    /// <summary>
    /// 静态服务提供商
    /// 主要用于不方便注入服务的情况
    /// </summary>
    public static IServiceProvider ServiceProvideR;
    public App()
    {

        
        if (!_initialized)
        {
            Log.Logger = SerilogIntegration.BuildSerilogInstance("Nice.Dotnet.Wpf");

            #region 全局异常捕获事件添加
            this.Startup += new StartupEventHandler(App_StartUpEvent);
            this.Exit += new ExitEventHandler(App_ExitEvent);
            #endregion


            _initialized =true;
        }
            
    }
    #region 全局异常捕获处理
    private void App_StartUpEvent(object sender, StartupEventArgs e)
    {
        //UI线程未捕获异常处理事件
        this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandleException);
        //Task线程未捕获异常处理时间
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        //非UI线程未捕获异常处理事件
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandlerException);
    }
    /// <summary>
    /// 处理程序退出后业务的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void App_ExitEvent(object sender, ExitEventArgs e)
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void App_DispatcherUnhandleException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        try
        {
            //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出
            e.Handled = true;
            Log.Warning($"捕获未处理异常：{e.Exception.InnerException}/{e.Exception.Message}");
            MessageBox.Show("发生错误请查看日志");
        }
        catch (Exception ex)
        {
            MessageBox.Show("程序发生致命错误，将停止运行");
            Log.Error($"程序发生致命错误，将停止运行：{e.Exception.InnerException}/{ex.Message}");
        }
    }

    private void CurrentDomain_UnhandlerException(object sender, UnhandledExceptionEventArgs e)
    {
        StringBuilder sbEx = new();

        if (e.IsTerminating)
        {
            sbEx.Append("程序发生致命错误，将终止！\n");
        }
        sbEx.Append("捕获未处理异常：");
        if (e.ExceptionObject is Exception exception)
        {
            sbEx.Append(exception.Message);
        }
        else
        {
            sbEx.Append(e.ExceptionObject);
        }
        Log.Error(sbEx.ToString());

        MessageBox.Show("程序发生致命错误，将终止");
    }
    void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        //MessageBox.Show("发生错误请查看日志");
        //task线程内未处理捕获
        Log.Warning($"捕获线程内未处理异常：{e.Exception.Message}");
        e.SetObserved();//设置该异常已察觉（这样处理后就不会引起程序崩溃）
    }
    #endregion

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        LiveCharts.Configure(config =>
            config
                // registers SkiaSharp as the library backend
                // REQUIRED unless you build your own
                .AddSkiaSharp()
                // adds the default supported types
                // OPTIONAL but highly recommend
                .AddDefaultMappers()
                // select a theme, default is Light
                .AddLightTheme()
            );
       await UseHostBuilder(e.Args);
    }
    private async Task UseHostBuilder(string[] args)
    {
        var hostBuilder = CreateHostBuilder(args);
        var host =await hostBuilder.StartAsync();
        ServiceProvideR = host.Services;
        host.Services.GetService<MainWindow>()?.ShowDialog();
        Current.Shutdown();
    }
    /// <summary>
    /// 使用Host作为配置
    /// </summary>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args).UseSerilog();
        hostBuilder.ConfigureServices((ctx, services) => {
            services.AddService();
        });
        return hostBuilder;
    }

    /// <summary>
    /// 使用默认Ioc容器注册服务
    /// </summary>
    private void DefaultStartUp()
    {
        //InitializeComponent();
        var services = new ServiceCollection();
        services.AddService();
        Ioc.Default.ConfigureServices(services.BuildServiceProvider());
    }

}
