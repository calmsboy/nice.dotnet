using Nice.Dotnet.Core.Config;

namespace Nice.Dotnet.Core.HttpClientStores
{
    /// <summary>
    /// 用户信息请求客户端
    /// </summary>
    public class CustomInfoClient
    {
        private readonly HttpClient _httpClient;

        public CustomInfoClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri($"https://{RequestUrlStore.APIHost}");
            _httpClient = httpClient??throw new ArgumentNullException($"{nameof(httpClient)}未注册该服务，请检查");
        }
        /// <summary>
        /// 获取客户端实例
        /// </summary>
        /// <returns></returns>
        public HttpClient GetInstance() => _httpClient;
    }
}
