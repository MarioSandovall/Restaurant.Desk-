using Model.Models.Login;
using Repository.Extensions;
using Repository.Interfaces;
using Service.Interfaces;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public AccountRepository(IWebService webService)
            : base(webService, "Account")
        {

        }

        public async Task<bool> ExistsAsync(string email)
        {
            var url = $"{Controller}/email/{email}";
            return await WebService.Client.GetAsync(url).ReadAsAsync<bool>();
        }

        public async Task<UserAccount> GetUserAccountAsync(string email)
        {
            var url = $"{Controller}/{email}";
            return await WebService.Client.GetAsync(url).ReadAsAsync<UserAccount>();
        }
    }
}
