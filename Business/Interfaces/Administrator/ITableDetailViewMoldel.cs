using Model.Models;
using System.Collections.Generic;

namespace Business.Interfaces.Administrator
{
    public interface ITableDetailViewMoldel
    {
        void Open(Table table, ICollection<BranchOffice> offices);
    }
}
