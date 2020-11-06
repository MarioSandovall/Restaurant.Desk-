using Model.Models;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface IDataService
    {
        User User { get; }
        Restaurant Restaurant { get; }
        BranchOffice CurrentOffice { get; }
        CashRegister CashRegister { get; }
        ICollection<PaymentType> PaymentTypes { get; }
        ICollection<BranchOffice> BranchOffices { get; }

        bool IsAdmin { get; }
        bool IsCasher { get; }
        void SetUser(User user);
        void SetCashRegister(CashRegister register);
        void SetBranchOffice(BranchOffice branchOffice);
        void SetRestaurant(Restaurant restaurant);
        void SetPaymentTypes(ICollection<PaymentType> types);
        void SetBranchOffices(ICollection<BranchOffice> branchOffices);
    }
}
