using System.ComponentModel.DataAnnotations;

namespace Model.Models
{
    public class Restaurant
    {


        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string Name { get; set; }

        public byte[] Image { get; set; }


        public Restaurant(Restaurant model)
        {
            Id = model.Id;
            Name = model.Name;
            //Image = model.Image ?? ImageHelper.RestaurantLogo.ImgUrlToByteArray();
        }

        public Restaurant()
        {

        }
    }
}
