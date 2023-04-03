using Nice.Common.Tool.IdCreate;
using Nice.Dotnet.Domain.IEntities;
using System.ComponentModel.DataAnnotations;

namespace Nice.Dotnet.Domain.Entities
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    public class CustomInfoModel : IEntity<string>
    {
        [Key]
        public string Id { get; set; } = IdCreater.GetBanksId();

        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string? City { get; set; }
    }
}
