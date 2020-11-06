using Model.Models;
using System.Collections.Generic;

namespace Business.Interfaces.Register
{
    public interface IProductListViewModel
    {
        ICollection<OrderDetail> Deatils { get; }
        void Load(ICollection<OrderDetail> details);
    }
}
