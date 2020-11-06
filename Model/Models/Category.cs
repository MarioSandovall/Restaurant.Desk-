using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(50, ErrorMessage = "Longitud máxima 50")]
        public string Name { get; set; }

        [StringLength(2000, ErrorMessage = "Longitud máxima 2000")]
        public string Description { get; set; }

        public bool Active { get; set; }

        public byte[] Image { get; set; }

        public int RestaurantId { get; set; }

        public void Update(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Active = category.Active;
            Description = category.Description;
            Image = category.Image;
        }

    }
}
