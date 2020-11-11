using Model.Dtos;
using Model.Interfaces;
using Model.Models;
using Repository.Extensions;
using Repository.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Repository.Repositories
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(IWebService webService)
            : base(webService, "Order") { }

        public async Task<IValueResponse<ICollection<OrderDetail>>> GetDetailsByIdAsync(int orderId)
        {
            var url = $"{Controller}/Details?orderId={orderId}";
            var response = await WebService.GetAsync(url);
            return await response.ReadHttpContentAsync<ICollection<OrderDetail>>();
        }

        public async Task<IValueResponse<Order>> CreateAsync(Order order, ICollection<OrderDetail> orderDetails)
        {
            var url = $"{Controller}";
            var response = await WebService.PostAsync(url, new { order, orderDetails });
            return await response.ReadHttpContentAsync<Order>();
        }

        public async Task<IValueResponse<ICollection<Order>>> GetOrdersByCashRegisterId(int cashRegisterId)
        {
            var url = $"{Controller}/CashRegister/{cashRegisterId}";
            var response = await WebService.GetAsync(url);
            return await response.ReadHttpContentAsync<ICollection<Order>>();
        }

        public async Task<IValueResponse<ICollection<TicketDetailDto>>> ChargeAsync(int orderId, int orderNumber, int restaurantId, int cashierId, int paymentType)
        {
            var url = $"{Controller}/Charge";
            var response = await WebService.PutAsync(url, new { orderId, orderNumber, cashierId, paymentType, restaurantId });

            return await response.ReadHttpContentAsync<ICollection<TicketDetailDto>>();
        }

        public async Task<IValueResponse<ICollection<TicketDetailDto>>> GetTicketAsync(int restaurantId, int orderNumber)
        {
            var url = $"{Controller}/Ticket?restaurantId={restaurantId}&orderNumber={orderNumber}";
            var response = await WebService.GetAsync(url);
            return await response.ReadHttpContentAsync<ICollection<TicketDetailDto>>();
        }

        public async Task<IResponse> CancelAsync(int orderId, int cashierId)
        {
            var url = $"{Controller}/cancel";
            var response = await WebService.PutAsync(url, new { orderId, cashierId });
            return response.ReadHttpContentAsync();
        }

    }
}
