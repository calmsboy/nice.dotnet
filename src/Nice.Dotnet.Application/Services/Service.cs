using Microsoft.EntityFrameworkCore;
using Nice.Dotnet.Application.IServices;
using Nice.Dotnet.Domain.Std.IEntities;
using Serilog;
using System.Linq.Expressions;

namespace Nice.Dotnet.Application.Services
{
    /// <summary>
    /// 基本CURD操作实现
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class Service<TDbContext,TModel, TKey> : IService<TModel, TKey> where TModel : class,IEntity<TKey> 
        where TDbContext : DbContext
    {
        public readonly TDbContext _dbContext;
        
        public Service(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async ValueTask<bool> AddAsync(TModel model)
        {
            await _dbContext.Set<TModel>().AddAsync(model);
            return await SaveAllChangeAsync();

        }
        
        public async ValueTask<bool> AddAsync(List<TModel> model)
        {
            await _dbContext.Set<TModel>().AddRangeAsync(model);
            return await SaveAllChangeAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await _dbContext.Set<TModel>().ToListAsync();
        }

        public async Task<TModel> GetAsync(TKey id)
        {
            return await _dbContext.Set<TModel>().Where(q=>q.Id.Equals(id)).FirstOrDefaultAsync()
                ?? throw new ArgumentNullException($"{nameof(id)}值为空！");
        }

        public async Task<IEnumerable<TModel>> GetEntitiesByConditionAsync(Expression<Func<TModel, bool>> query)
        {
            return await _dbContext.Set<TModel>().Where(query).ToListAsync();
        }

        public async ValueTask<bool> RemoveAsync(TKey id)
        {
             _dbContext.Set<TModel>().Remove(await _dbContext.Set<TModel>().FindAsync(id)
                 ??throw new ArgumentNullException("记录不存在,删除操作失败！"));
            return await SaveAllChangeAsync();
        }

        public async ValueTask<bool> SaveAllChangeAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"添加数据时发生错误：{ex.StackTrace}\n{ex.InnerException},{ex.Message}",ex);
                return false;
            }
        }

        public async ValueTask<bool> UpdateAsync(TModel model)
        {
            _dbContext.Set<TModel>().Update(model);
            return await SaveAllChangeAsync();
        }
    }
}
