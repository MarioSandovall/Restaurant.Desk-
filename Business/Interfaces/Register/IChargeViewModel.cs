using Model.Models;

namespace Business.Interfaces.Register
{
    public interface IChargeViewModel
    {
        void Open(Order order, decimal total);
    }
}
