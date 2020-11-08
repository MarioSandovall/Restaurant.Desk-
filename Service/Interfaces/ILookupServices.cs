using Model.Models;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface ILookupServices
    {
        IEnumerable<LookupItem> GetMenuItems();

        IEnumerable<LookupItem> GetOptions();
    }
}
