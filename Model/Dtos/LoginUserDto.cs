using Model.Models;
using System.Collections.Generic;

namespace Model.Dtos
{
    public class LoginUserDto
    {
        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<BranchOffice> BranchOffices { get; set; }
    }
}
