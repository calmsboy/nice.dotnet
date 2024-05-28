using Microsoft.EntityFrameworkCore;
using Nice.Dotnet.Application.IServices;
using Nice.Dotnet.Extension.LoggerSupport;
using Nice.Dotnet.WebApi.Controllers;
using Nice.Dotnet.WebApi.Models;
using Nice.Dotnet.WebApi.Services;
using Serilog;

namespace Nice.Dotnet.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = SerilogIntegration.BuildSerilogInstance("Nice.Dotnet.WebApi");
            // Add services to the container.

            builder.Services.AddControllers();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSignalR();
            builder.Services.AddDbContext<NiceDbContext>(opt =>
            {
                opt.UseSqlite(@"Data Source=Nice.db")
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) => false)));
            });

            builder.Services.AddScoped<ICustomInfoService, CustomInfoService>();

            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(builder =>
                        builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials())
            );

            builder.Host.UseSerilog();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<NiceMessageHub>("/api/chat");//apiµÿ÷∑
            app.Run();
        }
    }
}