using System.ComponentModel.DataAnnotations;

namespace Model.Models
{
    public class Table
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(50, ErrorMessage = "Longitud máxima 50")]
        public string Name { get; set; }

        public bool IsBussy { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Campo Requerido")]
        public int BranchOfficeId { get; set; }

        public byte[] Image { get; set; }

        public string OfficeName { get; set; }

    }
}
