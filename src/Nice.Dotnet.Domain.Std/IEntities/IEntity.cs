using System.Diagnostics.CodeAnalysis;

namespace Nice.Dotnet.Domain.Std.IEntities
{
    /// <summary>
    /// 实体建
    /// </summary>
    public interface IEntity<T>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [NotNull]
        T Id { get; }
    }
}
