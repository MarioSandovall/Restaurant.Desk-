using Model.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int OrderNumber { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(15, ErrorMessage = "Longitud máxima 15")]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int CashRegisterId { get; set; }

        public int RestaurantId { get; set; }

        public OrderStatusEnum Status { get; set; }

        public PaymentTypeEnum PaymentType { get; set; }

        public int? CashierId { get; set; }
        public User Cashier { get; set; }

        public Order() { }

        public Order(string name, int cashRegisterId)
        {
            Name = name;
            CashRegisterId = cashRegisterId;
        }
    }
}
