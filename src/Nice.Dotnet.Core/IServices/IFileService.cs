using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.Dotnet.Core.IServices
{
    public interface IFileService
    {

        /// <summary>
        /// 将文件读取成流
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Stream> OpenForReadFileAsync(string path);
        /// <summary>
        /// 将文件内容转换成实体类型
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<TValue> OpenFileToModel<TValue> (string path);
    }
}
