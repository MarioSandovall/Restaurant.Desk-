using Model.Models;
using Model.Models.Login;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface IDataService
    {
        User User { get; }
        
        LoginUser LoggedUser { get; }

        Restaurant Restaurant { get; }

        BranchOffice CurrentOffice { get; }

        CashRegister CashRegister { get; }

        ICollection<PaymentType> PaymentTypes { get; }

        ICollection<BranchOffice> BranchOffices { get; }

        bool IsAdmin { get; }

        bool IsCasher { get; }

        void SetUser(User user);

        void SetLoggedUser(LoginUser loggedUser);

        void SetCashRegister(CashRegister register);

        void SetBranchOffice(BranchOffice branchOffice);

        void SetRestaurant(Restaurant restaurant);

        void SetPaymentTypes(ICollection<PaymentType> types);

        void SetBranchOffices(ICollection<BranchOffice> branchOffices);
    }
}
