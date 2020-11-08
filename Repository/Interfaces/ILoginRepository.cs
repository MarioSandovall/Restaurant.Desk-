using Model.Models.Login;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ILoginRepository
    {

        Task<bool> ExistsAsync(AuthenticateModel authenticateModel);

        Task<LoginUser> GetUserAsync(AuthenticateModel authenticateModel);

    }
}
