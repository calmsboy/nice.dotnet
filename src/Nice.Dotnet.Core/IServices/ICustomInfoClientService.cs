using Nice.Dotnet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.Dotnet.Core.IServices
{
    /// <summary>
    /// 用户信息客户端接口
    /// </summary>
    public interface ICustomInfoClientService
    {
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        Task<List<CustomInfoModel>> GetCustomInfos();
        /// <summary>
        /// 添加一组用户信息
        /// </summary>
        /// <param name="customs"></param>
        /// <returns></returns>
        ValueTask<bool> PostCustomEntities(List<CustomInfoModel> customs);
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <returns></returns>
        ValueTask<bool> ClearData();
        
    }
}
