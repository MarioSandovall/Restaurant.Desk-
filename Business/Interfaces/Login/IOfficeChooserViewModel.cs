using Model.Models;
using System.Collections.Generic;

namespace Business.Interfaces.Login
{
    public interface IOfficeChooserViewModel
    {
        void Open(IEnumerable<BranchOffice> branchOffices);
    }
}
