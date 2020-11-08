using Service.Interfaces;
using Service.Utils;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Service.Services
{
    public class WebService : IWebService
    {
        public WebService()
        {
            var url = $"{ConfigHelper.Server}/Api/";
            Client = new HttpClient() { BaseAddress = new Uri(url) };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpClient Client { get; }
    }
}
