using Model.Models.Login;
using Repository.Extensions;
using Repository.Interfaces;
using Service.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class LoginRepository : RepositoryBase, ILoginRepository
    {
        public LoginRepository(IWebService webService)
           : base(webService, "Login")
        {

        }

        public async Task<bool> ExistsAsync(AuthenticateModel authenticateModel)
        {
            var url = $"{Controller}/user";
            return await WebService.Client.PostAsJsonAsync(url, authenticateModel).ReadAsAsync<bool>();
        }

        public async Task<LoginUser> GetUserAsync(AuthenticateModel authenticateModel)
        {
            var url = $"{Controller}";
            return await WebService.Client.PostAsJsonAsync(url, authenticateModel).ReadAsAsync<LoginUser>();
        }
    }
}
