using Model.Models.Home;

namespace Business.Wrappers.Home
{
    public class ProfileUserWrapper : ModelWrapper<ProfileUser>
    {
        public int Id => Model.Id;

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

        public string FullName => $"{Name} {LastName}";

        public byte[] Image
        {
            get => GetValue<byte[]>();
            set => SetValue(value);
        }

        public ProfileUserWrapper(ProfileUser model) : base(model)
        {

        }
    }
}
