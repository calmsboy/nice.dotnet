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
    }
}
