using Model.Dtos;
using Model.Interfaces;
using Model.Models;
using Repository.Extensions;
using Repository.Interfaces;
using Service.Interfaces;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class TableRepository : RepositoryBase, ITableRepository
    {
        public TableRepository(IWebService webService)
            : base(webService, "Table") { }

        public async Task<IValueResponse<TablesAndOfficesDto>> GetTablesAsync(int restaurantId)
        {
            var url = $"{Controller}?restaurantId={restaurantId}";
            var response = await WebService.GetAsync(url);
            return await response.ReadHttpContentAsync<TablesAndOfficesDto>();
        }

        public async Task<IValueResponse<Table>> SaveTablesAsync(Table table)
        {
            var url = $"{Controller}";
            var response = await WebService.PostAsync(url, table);
            return await response.ReadHttpContentAsync<Table>();
        }

        public async Task<IValueResponse<int>> DeleteTablesAsync(int tableId)
        {
            var url = $"{Controller}?tableId={tableId}";
            var response = await WebService.DeleteAsync(url);
            return await response.ReadHttpContentAsync<int>();
        }

    }
}
