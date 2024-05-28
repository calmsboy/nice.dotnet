using GrpcService1.Services;
using GrpcService1.Support;
using Serilog;
using System.Net;

namespace GrpcService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel((context, opt) =>
            {
                opt.Listen(IPAddress.Loopback, 5600, listenOpt =>
                {
                    listenOpt.UseHttps(Path.Combine("cert", "server.pfx"),"123456");
                });
            });
            //日志
            //builder.Services.AddLogging(opt =>
            //{

            //    opt.AddSerilog(Log.Logger);

            //});
            Log.Logger = SerilogIntegration.BuildSerilogInstance("GrpcService1");
            builder.Host.UseSerilog();
            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            var app = builder.Build();
            app.UseCors("AllowAll");
            app.Use(async(context,next) =>
            {
                await next();
            });
            // Configure the HTTP request pipeline.
            app.MapGrpcService<GreeterService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}