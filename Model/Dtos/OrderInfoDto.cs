using Model.Utils;
using System;

namespace Model.Dtos
{
    public class OrderInfoDto
    {
        public int OrderNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public OrderStatusEnum Status { get; set; }
        public string StatusName { get; set; }
        public string PaymentName { get; set; }
        public decimal Total { get; set; }
    }
}
