using Model.Models;
using Model.Utils;
using Service.Interfaces;
using Service.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Service.Services
{
    public class LookupService : ILookupServices
    {
        private readonly IUserService _userService;
        public LookupService(IUserService userService)
        {
            _userService = userService;
        }

        public IEnumerable<LookupItem> GetMenuOptions()
        {
            yield return new LookupItem
            {
                Name = "Home",
                Image = MenuImages.Home,
                Action = MenuAction.GoToHome
            };

            if (_userService.IsCasher)
            {
                yield return new LookupItem
                {
                    Name = "Cash Register",
                    Image = MenuImages.CashRegister,
                    Action = MenuAction.GoToCashRegister
                };
            }

            if (_userService.IsAdmin)
            {
                yield return new LookupItem
                {
                    Name = "Admin",
                    Image = MenuImages.Admin,
                    Action = MenuAction.GoToAdmin
                };
            }

            yield return new LookupItem()
            {
                Name = "Profile",
                Image = MenuImages.Profile,
                Action = MenuAction.GoToUserInformation
            };
        }

        public IEnumerable<LookupItem> GetSettingsOptions()
        {
            yield return new LookupItem()
            {
                Name = "Update",
                Image = MenuImages.Update,
                Action = MenuAction.UpdateApplication,
            };

            yield return new LookupItem()
            {
                Name = "Settings",
                Image = MenuImages.Settings,
                Action = MenuAction.GoToSettings
            };

            yield return new LookupItem
            {
                Name = "Close",
                Image = MenuImages.Close,
                Action = MenuAction.Exit
            };
        }

        public IEnumerable<LookupItem> GetAllMenuOptions()
        {
            return GetMenuOptions().Concat(GetSettingsOptions());
        }
    }
}
