using Model.Dtos;
using Model.Interfaces;
using Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ICashRegisterRepository
    {
        Task<IResponse> SetCashRegisterAsync(CashRegister model);
        Task<IValueResponse<ICollection<OrderInfoDto>>> GetCashRegisterInfoAsync(int cashRegisterId);
        Task<IValueResponse<CashRegisterInfoDto>> GetCashRegisterByOfficesAsync(int officeId);
    }
}
