using Newtonsoft.Json;

namespace Nice.Dotnet.Core.Services;

public class BaseClientService
{
    /// <summary>
    /// 请求处理者
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public async Task<(bool, TValue?)> RequestHandler<TValue>(Func<HttpResponseMessage> func) 
    {
        var response=  func.Invoke();
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return (true, JsonConvert.DeserializeObject<TValue>(content)?? default);
        }
        return (false, default(TValue));

    }
}
