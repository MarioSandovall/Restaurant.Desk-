using Model.Dtos;
using Model.Interfaces;
using Model.Models;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITableRepository
    {
        Task<IValueResponse<Table>> SaveTablesAsync(Table table);

        Task<IValueResponse<int>> DeleteTablesAsync(int tableId);
        
        Task<IValueResponse<TablesAndOfficesDto>> GetTablesAsync(int restaurantId);
    }
}
