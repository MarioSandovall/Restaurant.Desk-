using Model.Dtos;
using Model.Interfaces;
using Model.Models;
using Repository.Extensions;
using Repository.Interfaces;
using Service.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IWebService webService)
            : base(webService, "User")
        {

        }

        public async Task<IValueResponse<LoginUserDto>> LoginAsync(int userId, string password)
        {
            var url = $"{Controller}/login";
            var response = await WebService.Client.PostAsJsonAsync(url, new { userId, password });
            return await response.ReadHttpContentAsync<LoginUserDto>();
        }

        public async Task<IValueResponse<UsersWithRolesDto>> GetSystemUsersAsync(int restaurantId)
        {
            var url = $"{Controller}?restaurantId={restaurantId}";
            var response = await WebService.Client.GetAsync(url);
            return await response.ReadHttpContentAsync<UsersWithRolesDto>();

        }

        public async Task<IValueResponse<User>> ValidateEmailAsync(string email)
        {
            var url = $"{Controller}/email";
            var response = await WebService.Client.PostAsJsonAsync(url, email);
            return await response.ReadHttpContentAsync<User>();
        }

        public async Task<IValueResponse<User>> SaveUserAsync(User user)
        {
            var url = $"{Controller}";
            var response = await WebService.Client.PostAsJsonAsync(url, user);
            return await response.ReadHttpContentAsync<User>();
        }

        public async Task<IResponse> UpdateUserInfoAsync(User user)
        {
            var url = $"{Controller}/UserInfo";
            var response = await WebService.Client.PutAsJsonAsync(url, user);
            return response.ReadHttpContentAsync();
        }

        public async Task<IValueResponse<int>> DeleteUserAsync(int userId)
        {
            var url = $"{Controller}?Id={userId}";
            var response = await WebService.Client.DeleteAsync(url);
            return await response.ReadHttpContentAsync<int>();
        }

    }
}
