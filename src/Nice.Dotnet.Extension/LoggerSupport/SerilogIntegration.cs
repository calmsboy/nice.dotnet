using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Nice.Dotnet.Extension.LoggerSupport
{
    /// <summary>
    /// Serilog集成类
    /// </summary>
    public static class SerilogIntegration
    {
        /// <summary>
        /// 构建Serilog实例
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="theme"></param>
        /// <param name="isHaveDebugSkipWriteFile">是否在Debug模式跳过写入文件</param>
        /// <returns></returns>
        public static ILogger BuildSerilogInstance(string serverName, ConsoleTheme? theme = null, bool isHaveDebugSkipWriteFile = false)
        {
            #region LogSetup
            if (theme is null)
            {
                theme = AnsiConsoleTheme.Literate;
            }
            var loggerConfiguration = new LoggerConfiguration()
                 //.MinimumLevel.Debug()
                 .MinimumLevel.Override("System", LogEventLevel.Warning)
               //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)//.AspNetCore.Authentication
               //.MinimumLevel.Information()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .Enrich.FromLogContext()
               // 配置日志输出到文件，文件输出到当前项目的 logs 目录下
               // 日记的生成周期为每天
               .WriteTo.Async(f => f.File(Path.Combine("logs", @$"{serverName}.log")
                                , rollingInterval: RollingInterval.Day));

            if (!isHaveDebugSkipWriteFile)
            {
                loggerConfiguration.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}" +
                   "{Message:lj}{NewLine}{Exception}{NewLine}", theme: theme);
            }
            ILogger logger = loggerConfiguration.CreateLogger();
            return logger;
            #endregion
        }
    }
}
