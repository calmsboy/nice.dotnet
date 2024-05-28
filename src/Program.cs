using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;

namespace WebApplication2;

public class Program
{
  /// <summary>
  /// 自定义日志工厂，主要是筛选数据库操作日志
  /// </summary>
  public static readonly ILoggerFactory CustomLoggerFactory
  = LoggerFactory.Create(builder =>
  {
    builder
          .AddFilter((category, level) =>
              category == DbLoggerCategory.Database.Command.Name
              && (level == LogLevel.Warning || level == LogLevel.Error))
          .AddConsole();
  });
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    //配置数据库
    var dbConnect = builder.Configuration.GetConnectionString("PgSqlConnection");
    builder.Services.AddDbContextPool<ApplicationDbContext>(opt =>
    {
      opt.UseNpgsql(dbConnect
              ,
              option
                 => option.EnableRetryOnFailure())//.连接复原能力会自动重试失败的数据库命令()
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging()

           .UseLoggerFactory(CustomLoggerFactory);
    }, 28);

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();
    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseMigrationsEndPoint();
    }
    else
    {
      app.UseExceptionHandler("/Home/Error");
      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    app.Run();
  }
}
