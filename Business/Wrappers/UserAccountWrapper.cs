using Model.Models.Login;
using Service.Extensions;
using Service.Utils;

namespace Business.Wrappers
{
    public class UserAccountWrapper : ModelWrapper<UserAccount>
    {
        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string FullName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public byte[] Image
        {
            get => GetValue<byte[]>();
            set => SetValue(value);
        }

        public UserAccountWrapper(UserAccount model) : base(model)
        {
            Image = model.Image ?? Images.Profile.ImgUrlToByteArray();
        }
    }
}
