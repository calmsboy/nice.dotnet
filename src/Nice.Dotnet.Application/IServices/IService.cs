using Nice.Dotnet.Domain.Std.IEntities;
using System.Linq.Expressions;

namespace Nice.Dotnet.Application.IServices
{
    /// <summary>
    /// 通用CURD操作接口
    /// </summary>
    public interface IService<TModel,TKey> where TModel : class,IEntity<TKey>
    {
        /// <summary>
        /// 根据主键获取一条信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        Task<TModel> GetAsync(TKey id);
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TModel>> GetAllAsync();
        /// <summary>
        /// 获取满足条件的信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<TModel>> GetEntitiesByConditionAsync(Expression<Func<TModel,bool>> query);
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ValueTask<bool> AddAsync(TModel model);
        /// <summary>
        /// 添加一组记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ValueTask<bool> AddAsync(List<TModel> model);
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ValueTask<bool> UpdateAsync(TModel model);
        /// <summary>
        /// 移除一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        ValueTask<bool> RemoveAsync(TKey id);
        /// <summary>
        /// 保存所有改变
        /// </summary>
        /// <returns></returns>

        ValueTask<bool> SaveAllChangeAsync();

    }
}
