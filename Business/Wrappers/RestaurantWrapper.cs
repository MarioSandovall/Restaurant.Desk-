using Model.Models;

namespace Business.Wrappers
{
    public class RestaurantWrapper : ModelWrapper<Restaurant>
    {

        public int Id => Model.Id;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public byte[] Image
        {
            get => GetValue<byte[]>();
            set => SetValue(value);
        }


        public RestaurantWrapper(Restaurant model) : base(model)
        {

        }
    }
}
