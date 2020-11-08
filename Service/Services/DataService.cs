using Model.Models;
using Model.Utils;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Service.Services
{
    //TODO: Pending to remove
    public class DataService : IDataService
    {

        public User User { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsCasher { get; private set; }
        public CashRegister CashRegister { get; private set; }
        public BranchOffice CurrentOffice { get; private set; }
        public ICollection<PaymentType> PaymentTypes { get; private set; }
        public ICollection<BranchOffice> BranchOffices { get; private set; }
        public Restaurant Restaurant { get; private set; }

        public void SetUser(User user)
        {
            User = new User(user);
            IsAdmin = User.Roles.Contains((int)RoleEnum.Admin);
            IsCasher = User.Roles.Contains((int)RoleEnum.Cashier);
        }

        public void SetCashRegister(CashRegister register) => CashRegister = register;

        public void SetPaymentTypes(ICollection<PaymentType> types) => PaymentTypes = types;

        public void SetBranchOffice(BranchOffice branchOffice) => CurrentOffice = branchOffice;

        public void SetRestaurant(Restaurant restaurant)
        {
            Restaurant = new Restaurant(restaurant);
        }

        public void SetBranchOffices(ICollection<BranchOffice> branchOffices)
        {
            BranchOffices = branchOffices;
            if (BranchOffices.Any()) CurrentOffice = BranchOffices.First();
        }

    
    }
}
