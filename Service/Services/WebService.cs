using Service.Interfaces;
using Service.Utils;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Service.Services
{
    public class WebService : IWebService
    {
        private readonly HttpClient _client;
        public WebService()
        {
            var url = $"{ConfigHelper.Server}/Api/";
            _client = new HttpClient { BaseAddress = new Uri(url) };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return _client.GetAsync(requestUri);
        }

        public Task<HttpResponseMessage> PostAsync<T>(string requestUri, T value)
        {
            return _client.PostAsJsonAsync(requestUri, value);
        }

        public Task<HttpResponseMessage> PutAsync<T>(string requestUri, T value)
        {
            return _client.PutAsJsonAsync(requestUri, value);
        }

        public Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return _client.DeleteAsync(requestUri);
        }


        public void CancelPendingRequests()
        {
            _client.CancelPendingRequests();
        }


    }
}
