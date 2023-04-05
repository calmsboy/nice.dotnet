namespace Nice.Dotnet.Core.Config
{
    public partial class RequestUrlStore
    {
        /// <summary>
        /// 用户信息API地址名称
        /// </summary>

        public const string CustomInfoAPIName = "CustomInfo";

        public const string CustomInfoAPIBaseUrl = $"{APIBaseTemplete}/{CustomInfoAPIName}";
        /// <summary>
        /// 获取所有用户数据
        /// </summary>

        public const string CustomInfoGetAllUrl = $"{CustomInfoAPIBaseUrl}/list";
    }
}
