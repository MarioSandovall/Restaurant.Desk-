using Model.Models;
using System.Collections.Generic;

namespace Business.Interfaces.Administrator
{
    public interface IProductDetailViewModel
    {
        void Open(Product product, ICollection<Category> categories);
    }
}
