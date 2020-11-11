using Model.Models.Login;
using Model.Utils;
using Service.Extensions;
using Service.Interfaces;
using Service.Utils;
using System.Linq;

namespace Service.Services
{
    public class UserService : IUserService
    {
        public int Id { get; private set; }

        public byte[] Image { get; private set; }

        public string Email { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool IsCasher { get; private set; }

        public string Name { get; private set; }

        public string LastName { get; private set; }

        public string FullName => $"{Name} {LastName}";

        public string OfficeName { get; private set; }

        public string RestaurantName { get; private set; }

        public void SetUser(LoginUser user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            LastName = user.LastName;
            OfficeName = user.OfficeName;
            RestaurantName = user.RestaurantName;
            Image = user.Image ?? Images.Profile.ImgUrlToByteArray();
            IsAdmin = user.RoleIds.Contains((int)RoleEnum.Admin);
            IsCasher = user.RoleIds.Contains((int)RoleEnum.Cashier);
        }
    }
}
