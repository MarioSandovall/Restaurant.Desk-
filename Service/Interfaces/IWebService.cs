using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IWebService
    {

        void SetToken(string token);

        void CancelPendingRequests();

        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T value);

        Task<HttpResponseMessage> PutAsync<T>(string requestUri, T value);

        Task<HttpResponseMessage> DeleteAsync(string requestUri);
    }
}
