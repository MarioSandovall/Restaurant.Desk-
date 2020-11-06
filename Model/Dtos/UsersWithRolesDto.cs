using Model.Models;
using System.Collections.Generic;

namespace Model.Dtos
{
    public class UsersWithRolesDto
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}
