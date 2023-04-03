using Microsoft.EntityFrameworkCore;
using Nice.Dotnet.Domain.Entities;

namespace Nice.Dotnet.WebApi.Models
{
    /// <summary>
    /// Nice数据库上下文
    /// </summary>
    public class NiceDbContext : DbContext
    {
        public NiceDbContext(DbContextOptions options) : base(options)
        {
        }

        protected NiceDbContext()
        {
        }

        public DbSet<CustomInfoModel> CustomInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
