using Model.Models;
using System.Collections.Generic;

namespace Business.Interfaces.Register
{
    public interface IOrderViewModel
    {
        void Load(ICollection<Product> products, ICollection<Category> categories);

        void Open(Order order, ICollection<OrderDetail> details);
    }
}
