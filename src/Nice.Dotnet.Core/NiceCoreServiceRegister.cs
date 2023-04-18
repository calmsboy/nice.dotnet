using Microsoft.Extensions.DependencyInjection;
using Nice.Dotnet.Core.HttpClientStores;
using Nice.Dotnet.Core.IServices;
using Nice.Dotnet.Core.Services;

namespace Nice.Dotnet.Core;

public static class NiceCoreServiceRegister
{
    public static IServiceCollection AddNiceClientService(this IServiceCollection services,string baseUrl="https://localhost:5188"
        ,HttpMessageHandler? messageHandler= null)
    {
        var httpClient = messageHandler is null? new HttpClient():new HttpClient(messageHandler);

        httpClient.BaseAddress = new Uri(baseUrl);

        httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");

        services.AddSingleton(spa=> httpClient);
        services.AddSingleton<ICustomInfoClientService,CustomInfoClientService>();
        services.AddHttpClient();
        var chatClient = new ChatConnectService(baseUrl);
        services.AddSingleton(spa=> chatClient);
        return services;
    }
}
