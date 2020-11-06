using Model.Models;
using System.Collections.Generic;

namespace Model.Dtos
{
    public class TablesAndOfficesDto
    {
        public ICollection<Table> Tables { get; set; }
        public ICollection<BranchOffice> Offices { get; set; }
    }
}
