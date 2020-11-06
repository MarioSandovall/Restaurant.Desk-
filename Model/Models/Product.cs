using System.ComponentModel.DataAnnotations;

namespace Model.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(50, ErrorMessage = "Longitud máxima 50")]
        public string Name { get; set; }

        [StringLength(2000, ErrorMessage = "Longitud máxima 2000")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public decimal Price { get; set; }

        public bool Active { get; set; }

        public byte[] Image { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Campo Requerido")]
        public int ProductCategoryId { get; set; }
        public Category Category { get; set; }

        public int RestaurantId { get; set; }

        public string CategoryName { get; set; }

        public Product()
        {

        }

        public void Update(Product product)
        {
            Id = product.Id;
            Price = product.Price;
            Name = product.Name;
            Description = product.Description;
            Image = product.Image;
            Active = product.Active;
            Category = product.Category;
            ProductCategoryId = product.ProductCategoryId;
        }
    }
}
