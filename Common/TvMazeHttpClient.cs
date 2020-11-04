using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Interfaces;
using Newtonsoft.Json;

namespace Common
{
    public class TvMazeHttpClient: IHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public TvMazeHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = "http://api.tvmaze.com";
        }

        public async Task<IEnumerable<T>> GetAll<T>(string path)
        {
            var result = await _httpClient.GetStringAsync($"{_baseUrl}/{path}");

            return JsonConvert.DeserializeObject<List<T>>(result);
        }
    }
}