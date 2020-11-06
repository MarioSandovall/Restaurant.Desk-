using Model.Dtos;
using Model.Interfaces;
using Model.Models;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IValueResponse<LoginUserDto>> LoginAsync(int userId, string password);

        Task<IValueResponse<UsersWithRolesDto>> GetSystemUsersAsync(int restaurantId);

        Task<IValueResponse<User>> SaveUserAsync(User user);

        Task<IResponse> UpdateUserInfoAsync(User user);

        Task<IValueResponse<int>> DeleteUserAsync(int userId);

        Task<IValueResponse<User>> ValidateEmailAsync(string email);
    }
}
