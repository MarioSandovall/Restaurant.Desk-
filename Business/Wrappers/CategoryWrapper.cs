using Model.Models;

namespace Business.Wrappers
{
    public class CategoryWrapper : ModelWrapper<Category>
    {

        public int Id => Model.Id;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Description
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool Active
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public byte[] Image
        {
            get => GetValue<byte[]>();
            set => SetValue(value);
        }

        public int RestaurantId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }


        public CategoryWrapper(Category model)
            : base(model)
        {

        }

        public void Update(Category category)
        {
            Name = category.Name;
            Active = category.Active;
            Description = category.Description;
            Image = category.Image;
        }
    }
}
