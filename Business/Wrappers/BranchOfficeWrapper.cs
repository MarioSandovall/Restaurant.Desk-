using Model.Models;

namespace Business.Wrappers
{
    public class BranchOfficeWrapper : ModelWrapper<BranchOffice>
    {
        public int Id => Model.Id;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string StateProvince
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Town
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Suburb
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Street
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string OutdoorNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int PostalCode
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public bool Active
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public int RestaurantId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public BranchOfficeWrapper(BranchOffice model)
        : base(model)
        {

        }

        public void Update(BranchOffice office)
        {
            Name = office.Name;
            StateProvince = office.StateProvince;
            Town = office.Town;
            Suburb = office.Suburb;
            Street = office.Street;
            OutdoorNumber = office.OutdoorNumber;
            PostalCode = office.PostalCode;
            Active = office.Active;
            RestaurantId = office.RestaurantId;
        }
    }
}
