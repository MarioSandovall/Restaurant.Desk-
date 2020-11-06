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

        public async Task<UserAccount> GetUserAccountAsync(string email)
        {
            var url = $"{Controller}/{email}";
            var response = await WebService.Client.GetAsync(url);

            return await response.ReadAsAsync<UserAccount>();
        }
    }
}
