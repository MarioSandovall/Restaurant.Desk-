using System.ComponentModel.DataAnnotations;

namespace Model.Models
{
    public class BranchOffice
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(50, ErrorMessage = "Longitud máxima 50")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Longitud máxima 500")]
        public string StateProvince { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(500, ErrorMessage = "Longitud máxima 500")]
        public string Town { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(500, ErrorMessage = "Longitud máxima 500")]
        public string Suburb { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(500, ErrorMessage = "Longitud máxima 500")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(50, ErrorMessage = "Longitud máxima 50")]
        public string OutdoorNumber { get; set; }

        public int PostalCode { get; set; }

        public bool Active { get; set; }

        public int RestaurantId { get; set; }
    }
}
