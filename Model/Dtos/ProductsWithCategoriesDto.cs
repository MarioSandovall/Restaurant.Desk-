using Model.Models;
using System.Collections.Generic;

namespace Model.Dtos
{
    public class ProductsWithCategoriesDto
    {
        public ICollection<Product> Products { get; set; }
        public ICollection<Category> ProductCategories { get; set; }
    }
}
