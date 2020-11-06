using Model.Models;
using System.Collections.Generic;

namespace Business.Interfaces.Administrator
{
    public interface IUserDetailViewModel
    {
        void Open(User user, ICollection<Role> roles);
    }
}
