using System;

namespace Model.Models
{
    public class CashRegister
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal Quantity { get; set; }

        public int UserId { get; set; }

        public int BranchOfficeId { get; set; }
    }
}
