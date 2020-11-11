using Model.Models.Login;

namespace Service.Interfaces
{
    public interface IUserService
    {
        int Id { get; }

        byte[] Image { get; }

        string Email { get; }

        bool IsAdmin { get; }

        bool IsCasher { get; }

        string FullName { get; }

        string Name { get; }

        string LastName { get; }

        string OfficeName { get; }

        string RestaurantName { get; }

        void SetUser(LoginUser user);
    }
}
