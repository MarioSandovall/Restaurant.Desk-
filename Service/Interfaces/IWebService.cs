using System.Net.Http;

namespace Service.Interfaces
{
    public interface IWebService
    {
        HttpClient Client { get; }
    }
}
