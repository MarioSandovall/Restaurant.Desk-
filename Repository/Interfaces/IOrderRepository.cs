using Model.Dtos;
using Model.Interfaces;
using Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IResponse> CancelAsync(int orderId, int cashierId);
        Task<IValueResponse<ICollection<Order>>> GetOrdersByCashRegisterId(int cashRegisterId);
        Task<IValueResponse<ICollection<OrderDetail>>> GetDetailsByIdAsync(int orderId);
        Task<IValueResponse<Order>> CreateAsync(Order order, ICollection<OrderDetail> orderDetails);
        Task<IValueResponse<ICollection<TicketDetailDto>>> GetTicketAsync(int restaurantId, int orderNumber);
        Task<IValueResponse<ICollection<TicketDetailDto>>> ChargeAsync(int orderId, int orderNumber, int restaurantId, int cashierId, int paymentType);
    }
}
