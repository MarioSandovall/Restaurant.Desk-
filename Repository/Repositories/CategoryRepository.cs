using Model.Interfaces;
using Model.Models;
using Repository.Extensions;
using Repository.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {

        public CategoryRepository(IWebService webService)
            : base(webService, "Category") { }

        public async Task<IValueResponse<ICollection<Category>>> GetCategoriesAsync(int restaurantId)
        {

            var url = $"{Controller}?restaurantId={restaurantId}";
            var response = await WebService.GetAsync(url);
            return await response.ReadHttpContentAsync<ICollection<Category>>();
        }

        public async Task<IValueResponse<Category>> SaveCategoryAsync(Category category)
        {

            var url = $"{Controller}";
            var response = await WebService.PostAsync(url, category);
            return await response.ReadHttpContentAsync<Category>();

        }

        public async Task<IValueResponse<int>> DeleteCategoryAsync(int categoryId)
        {

            var url = $"{Controller}?categoryId={categoryId}";
            var response = await WebService.DeleteAsync(url);
            return await response.ReadHttpContentAsync<int>();

        }
    }
}
