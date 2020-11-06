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
    public class CashRegisterRepository : RepositoryBase, ICashRegisterRepository
    {
        public CashRegisterRepository(IWebService webService)
            : base(webService, "CashRegister") { }

        public async Task<IValueResponse<CashRegisterInfoDto>> GetCashRegisterByOfficesAsync(int officeId)
        {
            var url = $"{Controller}?officeId={officeId}";
            var response = await WebService.Client.GetAsync(url);
            return await response.ReadHttpContentAsync<CashRegisterInfoDto>();
        }

        public async Task<IResponse> SetCashRegisterAsync(CashRegister model)
        {
            var url = $"{Controller}";
            var response = await WebService.Client.PostAsJsonAsync(url, model);
            return response.ReadHttpContentAsync();
        }

        public async Task<IValueResponse<ICollection<OrderInfoDto>>> GetCashRegisterInfoAsync(int cashRegisterId)
        {
            var url = $"{Controller}/Info/{cashRegisterId}";
            var response = await WebService.Client.GetAsync(url);
            return await response.ReadHttpContentAsync<ICollection<OrderInfoDto>>();
        }

    }
}
