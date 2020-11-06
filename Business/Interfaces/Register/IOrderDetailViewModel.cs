using Model.Models;
using System.Threading.Tasks;

namespace Business.Interfaces.Register
{
    public interface IOrderDetailViewModel
    {
        Order Order { get; }
        
        Task LoadAsync(Order order);
    }
}
