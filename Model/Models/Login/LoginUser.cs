using System.Collections.Generic;

namespace Model.Models.Login
{
    public class LoginUser
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public byte[] Image { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string OfficeName { get; private set; }

        public string RestaurantName { get; private set; }

        public IEnumerable<int> RoleIds { get; set; }
    }
}
