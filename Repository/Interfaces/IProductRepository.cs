using Model.Dtos;
using Model.Interfaces;
using Model.Models;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IValueResponse<Product>> SaveProductAsync(Product product);

        Task<IValueResponse<ProductsWithCategoriesDto>> GetProductsWithCategoriesAsync(int restaurantId);
        
        Task<IValueResponse<int>> DeleteProductAsync(int productId);
    }
}
