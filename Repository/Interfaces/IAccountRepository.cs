using Model.Models.Login;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> ExistsAsync(string email);

        Task<UserAccount> GetUserAccountAsync(string email);
    }
}
