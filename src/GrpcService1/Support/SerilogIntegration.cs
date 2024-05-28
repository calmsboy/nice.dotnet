
using Serilog;
using Serilog.Events;

namespace GrpcService1.Support;

/// <summary>
/// Serilog集成类
/// </summary>
public static class SerilogIntegration
{
    /// <summary>
    /// 构建Serilog实例
    /// </summary>
    /// <param name="serverName">服务名称</param>
    /// <param name="isHaveDebugSkipWriteConsole">是否在Debug模式跳过写入控制台</param>
    /// <returns></returns>
    public static Serilog.ILogger BuildSerilogInstance(string serverName, bool isHaveDebugSkipWriteConsole = false)
    {
        string outputTemplete = "{Timestamp: yyyy-MM-dd HH:mm:ss} || [{Level}] || {SourceContext:l} || {Message} || {Exception} ||end {NewLine}";
        #region LogSetup
        var loggerConfiguration = new Serilog.LoggerConfiguration()
             //.MinimumLevel.Debug()
             .MinimumLevel.Override("System", LogEventLevel.Debug)
           .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)//.AspNetCore.Authentication
           .MinimumLevel.Debug()
           .Enrich.FromLogContext()
           .WriteTo.Console()
           // 配置日志输出到文件，文件输出到当前项目的 logs 目录下
           // 日记的生成周期为每天
           .WriteTo.Async(f => f.File(Path.Combine(AppContext.BaseDirectory, "logs", $"{serverName}.log")
                            , rollingInterval: RollingInterval.Day, outputTemplate: outputTemplete));

        Serilog.ILogger logger = loggerConfiguration.CreateLogger();
        return logger;
        #endregion
    }
}
