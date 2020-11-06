using Model.Models;
using System.Collections.Generic;

namespace Model.Dtos
{
    public class CashRegisterInfoDto
    {
        public CashRegister CashRegister { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<PaymentType> PaymentTypes { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
