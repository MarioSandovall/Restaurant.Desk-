using Model.Models.Login;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserAccount> GetUserAccountAsync(string email);
    }
}
