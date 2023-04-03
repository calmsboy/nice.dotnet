using Microsoft.EntityFrameworkCore;
using Nice.Dotnet.Application.IServices;
using Nice.Dotnet.Extension.LoggerSupport;
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

            builder.Services.AddDbContext<NiceDbContext>(opt =>
            {
                opt.UseSqlite(@"Data Source=Nice.db");
            });

            builder.Services.AddScoped<ICustomInfoService, CustomInfoService>();

            builder.Host.UseSerilog();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}