using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(25, ErrorMessage = "Longitud máxima 25")]
        public string Name { get; set; }

        [StringLength(25, ErrorMessage = "Longitud máxima 25")]
        public string LastName { get; set; }

        public string FullName => $"{Name} {LastName}";

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(25, ErrorMessage = "Longitud máxima 25")]
        public string Password { get; set; }

        public byte[] Image { get; set; }

        public bool Active { get; set; }

        public int RestaurantId { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "Correo no valido")]
        [StringLength(50, ErrorMessage = "Longitud máxima 50")]
        public string Email { get; set; }

        public string RoleNames { get; set; }

        public ICollection<int> Roles { get; set; }


        public User() { }

        public User(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Roles = user.Roles;
            Email = user.Email;
            Active = user.Active;
            LastName = user.LastName;
            Password = user.Password;
            //Image = user.Image ?? ImageHelper.ProfileImg.ImgUrlToByteArray();
        }
    }
}
