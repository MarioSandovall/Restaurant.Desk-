using Model.Interfaces;
using Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<IResponse> UpdateRestaurantAsync(Restaurant restaurant);

        Task<IValueResponse<ICollection<BranchOffice>>> GetOfficesAsync(int restaurantId);

        Task<IValueResponse<BranchOffice>> SaveBranchOfficeAsync(BranchOffice office);

        Task<IValueResponse<int>> DeleteBranchOfficeAsync(int officeId);
    }
}
