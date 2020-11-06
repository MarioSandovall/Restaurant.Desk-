using Model.Models;
using System.Collections.Generic;

namespace Business.Interfaces.Register
{
    public interface ISearchProductViewModel
    {
        void Load(ICollection<Product> products, ICollection<Category> categories);
    }
}
