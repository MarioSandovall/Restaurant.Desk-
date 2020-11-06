using Model.Models;
using System.Collections.Generic;

namespace Business.Interfaces.Register
{
    public interface IOrderListViewModel
    {
        void Load(ICollection<Order> orders);
    }
}
