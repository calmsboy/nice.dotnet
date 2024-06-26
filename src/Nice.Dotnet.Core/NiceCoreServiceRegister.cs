﻿using Microsoft.Extensions.DependencyInjection;
using Nice.Dotnet.Core.HttpClientStores;
using Nice.Dotnet.Core.IServices;
using Nice.Dotnet.Core.Services;

namespace Nice.Dotnet.Core;

public static class NiceCoreServiceRegister
{
    public static IServiceCollection AddNiceClientService(this IServiceCollection services,string baseUrl="https://localhost:5188"
        ,HttpMessageHandler? messageHandler= null,string clientName = "DefaultHttpClient")
    {
        var httpClientBuilder = services.AddHttpClient(clientName, opt =>
        {
            opt.BaseAddress = new Uri(baseUrl);
            opt.DefaultRequestHeaders.Connection.Add("keep-alive");
        });
        if(messageHandler is not null)
            httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => messageHandler);

        services.AddSingleton<ICustomInfoClientService,CustomInfoClientService>();

        var chatClient = new ChatConnectService(baseUrl);
        services.AddSingleton(spa=> chatClient);
        return services;
    }
}
