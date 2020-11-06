using Model.Dtos;
using Model.Interfaces;
using Model.Models;
using Repository.Extensions;
using Repository.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        public ProductRepository(IWebService webService) : base(webService, "Product") { }

        public async Task<IValueResponse<ICollection<Product>>> GetProductsAsync(int restaurantId)
        {

            var url = $"{Controller}?restaurantId={restaurantId}";
            var response = await WebService.Client.GetAsync(url);
            return await response.ReadHttpContentAsync<ICollection<Product>>();

        }

        public async Task<IValueResponse<ProductsWithCategoriesDto>> GetProductsWithCategoriesAsync(int restaurantId)
        {

            var url = $"{Controller}/categories?restaurantId={restaurantId}";
            var response = await WebService.Client.GetAsync(url);
            return await response.ReadHttpContentAsync<ProductsWithCategoriesDto>();

        }

        public async Task<IValueResponse<Product>> SaveProductAsync(Product product)
        {

            var url = $"{Controller}";
            var response = await WebService.Client.PostAsJsonAsync(url, product);
            return await response.ReadHttpContentAsync<Product>();

        }

        public async Task<IValueResponse<int>> DeleteProductAsync(int productId)
        {

            var url = $"{Controller}?productId={productId}";
            var response = await WebService.Client.DeleteAsync(url);
            return await response.ReadHttpContentAsync<int>();

        }
    }
}
