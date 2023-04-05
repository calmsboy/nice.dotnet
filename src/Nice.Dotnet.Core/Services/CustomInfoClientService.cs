using Nice.Dotnet.Core.Config;
using Nice.Dotnet.Core.HttpClientStores;
using Nice.Dotnet.Core.IServices;
using Nice.Dotnet.Domain.Entities;
using System.Net.Http.Json;

namespace Nice.Dotnet.Core.Services
{
    public class CustomInfoClientService : BaseClientService, ICustomInfoClientService
    {

        private readonly HttpClient _httpClient;

        public CustomInfoClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async ValueTask<bool> ClearData()
        {
            var response = await _httpClient.DeleteAsync(RequestUrlStore.CustomInfoAPIBaseUrl);

            var (suc, entities) = await RequestHandler<bool>(() => response);

            return entities;
        }

        public async Task<List<CustomInfoModel>> GetCustomInfos()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CustomInfoModel>>(RequestUrlStore.CustomInfoGetAllUrl);
            return response ?? new List<CustomInfoModel>();
        }

        public async ValueTask<bool> PostCustomEntities(List<CustomInfoModel> customs)
        {
            var response = await _httpClient
                .PostAsJsonAsync(RequestUrlStore.CustomInfoAPIBaseUrl,customs);

            var (suc, entities) = await RequestHandler<bool>(() => response);

            return entities;
        }
    }
}
