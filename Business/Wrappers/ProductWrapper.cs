using Business.Events.Register;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using System.Windows.Input;

namespace Business.Wrappers
{
    public class ProductWrapper : ModelWrapper<Product>
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

        public string CategoryName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal Price
        {
            get => GetValue<decimal>();
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

        public int ProductCategoryId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public Category Category
        {
            get => GetValue<Category>();
            set => SetValue(value);
        }

        public int RestaurantId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public ICommand AddProductCommand { get; set; }


        public ProductWrapper(Product model) : base(model)
        {
            //Image = model.Image ?? ImageHelper.ProductImg.ImgUrlToByteArray();
        }

        public ProductWrapper(Product model, IEventAggregator eventAggregator) : base(model)
        {
            AddProductCommand = new DelegateCommand(() => eventAggregator.GetEvent<AddProductToProductListEvent>().Publish(Model));
        }

        public void Update(Product product)
        {
            Price = product.Price;
            Name = product.Name;
            Description = product.Description;
            Image = product.Image;
            Active = product.Active;
            Category = product.Category;
            ProductCategoryId = product.ProductCategoryId;
            CategoryName = product.CategoryName;
        }

    }
}
