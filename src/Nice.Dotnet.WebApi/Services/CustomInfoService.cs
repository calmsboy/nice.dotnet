using Microsoft.EntityFrameworkCore;
using Nice.Dotnet.Application.IServices;
using Nice.Dotnet.Application.Services;
using Nice.Dotnet.Domain.Entities;
using Nice.Dotnet.WebApi.Models;

namespace Nice.Dotnet.WebApi.Services
{
    public class CustomInfoService : Service<NiceDbContext, CustomInfoModel, string>, ICustomInfoService
    {
        public CustomInfoService(NiceDbContext dbContext) : base(dbContext)
        {
        }

        public async ValueTask ClearData()
        {
            var entities = await GetAllAsync();
            _dbContext.Set<CustomInfoModel>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}
