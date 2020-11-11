using Model.Interfaces;
using Model.Models;
using Repository.Extensions;
using Repository.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class RestaurantRepository : RepositoryBase, IRestaurantRepository
    {
        public RestaurantRepository(IWebService webService)
            : base(webService, "Restaurant")
        {

        }

        public async Task<IValueResponse<ICollection<BranchOffice>>> GetOfficesAsync(int restaurantId)
        {
            var url = $"{Controller}/BranchOffice?restaurantId={restaurantId}";
            var response = await WebService.GetAsync(url);
            return await response.ReadHttpContentAsync<ICollection<BranchOffice>>();
        }

        public async Task<IResponse> UpdateRestaurantAsync(Restaurant restaurant)
        {
            var url = $"{Controller}";
            var response = await WebService.PutAsync(url, restaurant);
            return response.ReadHttpContentAsync();
        }

        public async Task<IValueResponse<BranchOffice>> SaveBranchOfficeAsync(BranchOffice office)
        {
            var url = $"{Controller}/BranchOffice";
            var response = await WebService.PostAsync(url, office);
            return await response.ReadHttpContentAsync<BranchOffice>();
        }

        public async Task<IValueResponse<int>> DeleteBranchOfficeAsync(int officeId)
        {
            var url = $"{Controller}/BranchOffice?officeId={officeId}";
            var response = await WebService.DeleteAsync(url);
            return await response.ReadHttpContentAsync<int>();
        }


    }
}
