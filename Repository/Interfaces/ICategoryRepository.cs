using Model.Interfaces;
using Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IValueResponse<int>> DeleteCategoryAsync(int categoryId);

        Task<IValueResponse<Category>> SaveCategoryAsync(Category category);
        
        Task<IValueResponse<ICollection<Category>>> GetCategoriesAsync(int restaurantId);
    }
}
