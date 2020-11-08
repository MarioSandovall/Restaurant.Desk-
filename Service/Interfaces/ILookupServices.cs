using Model.Models;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface ILookupServices
    {
        IEnumerable<LookupItem> GetMenuOptions();

        IEnumerable<LookupItem> GetSettingsOptions();

        IEnumerable<LookupItem> GetAllMenuOptions();
    }
}
