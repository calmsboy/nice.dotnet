namespace Nice.Dotnet.Domain.Std.IEntities
{
    public interface ICustomInfoRoot:IEntity<string>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        string? Name { get; }
        /// <summary>
        /// 城市
        /// </summary>
        string? City { get; }
    }
}
