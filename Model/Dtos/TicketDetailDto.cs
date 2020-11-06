using System;

namespace Model.Dtos
{
    public class TicketDetailDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Product { get; set; }
        public int OrderNumber { get; set; }
        public string Cashier { get; set; }
    }
}
