using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Idt.Data;
namespace WebApp.Idt;

public class Program
{
    /// <summary>
    /// �Զ�����־��������Ҫ��ɸѡ���ݿ������־
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

        //�������ݿ�
        //var dbConnect = builder.Configuration.GetConnectionString("PgSqlConnection");
        //builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        //{
        //    opt.UseNpgsql(dbConnect
        //            ,
        //            option
        //               => option.EnableRetryOnFailure())//.���Ӹ�ԭ�������Զ�����ʧ�ܵ����ݿ�����()
        //         .EnableDetailedErrors()
        //         .EnableSensitiveDataLogging()

        //         .UseLoggerFactory(CustomLoggerFactory);
        //});
        var dbConnect = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(dbConnect).UseLoggerFactory(CustomLoggerFactory);
        });
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
