using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Idt.Data;

public class ApplicationDbContext : IdentityDbContext
{
    private readonly IConfiguration _configuration;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IConfiguration configuration)
        : base(options)
    {
        _configuration=configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //配置数据库
        var dbConnect = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(dbConnect);
        base.OnConfiguring(optionsBuilder);
    }
}
