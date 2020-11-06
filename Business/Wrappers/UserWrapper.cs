using Model.Models;
using Service.Extensions;
using Service.Utils;
using System.Collections.Generic;

namespace Business.Wrappers
{
    public class UserWrapper : ModelWrapper<User>
    {

        public int Id => Model.Id;

        public string FullName => Name + " " + LastName;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Password
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public byte[] Image
        {
            get => GetValue<byte[]>();
            set => SetValue(value);
        }

        public bool Active
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }


        public string RoleNames
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ICollection<int> Roles
        {
            get => GetValue<ICollection<int>>();
            set => SetValue(value);
        }

        public UserWrapper(User model) : base(model)
        {
            Image = model.Image ?? RestaurantImages.Profile.ImgUrlToByteArray();
        }

        public void Update(User user)
        {
            Name = user.Name;
            LastName = user.LastName;
            Password = user.Password;
            Image = user.Image;
            Active = user.Active;
            Roles = user.Roles;
            RoleNames = user.RoleNames;
        }
    }
}
