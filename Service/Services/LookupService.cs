using Model.Models;
using Model.Utils;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Service.Services
{
    public class LookupService : ILookupServices
    {
        private readonly IUserService _userService;
        public LookupService(
            IUserService userService)
        {
            _userService = userService;
        }

        public IEnumerable<LookupItem> GetMenuOptions()
        {
            yield return new LookupItem()
            {
                Name = "Home",
                IsForAdmin = false,
                Action = MenuAction.GoToHome,
                Image = "/Wpf;component/Resources/Images/Menu/Home.png",
            };

            if (_userService.IsCasher)
            {
                yield return new LookupItem()
                {
                    Name = "Cash Register",
                    IsForAdmin = false,
                    Action = MenuAction.GoToCashRegister,
                    Image = "/Wpf;component/Resources/Images/Menu/CountingMachine.png",
                };
            }

            if (_userService.IsAdmin)
            {
                yield return new LookupItem()
                {
                    Name = "Admin",
                    IsForAdmin = true,
                    Action = MenuAction.GoToAdmin,
                    Image = "/Wpf;component/Resources/Images/Menu/Admin.png",
                };
            }

            yield return new LookupItem()
            {
                Name = "Profile",
                IsForAdmin = false,
                Action = MenuAction.GoToUserInformation,
                Image = "/Wpf;component/Resources/Images/Menu/UserProfile.png",
            };
        }

        public IEnumerable<LookupItem> GetSettingsOptions()
        {
            yield return new LookupItem()
            {
                Name = "Update",
                IsForAdmin = false,
                Action = MenuAction.UpdateApplication,
                Image = "/Wpf;component/Resources/Images/Menu/Update.png",
            };

            yield return new LookupItem()
            {
                Name = "Settings",
                IsForAdmin = false,
                Action = MenuAction.GoToSettings,
                Image = "/Wpf;component/Resources/Images/Menu/Settings.png",
            };

            yield return new LookupItem()
            {
                Name = "Close",
                IsForAdmin = false,
                Action = MenuAction.Exit,
                Image = "/Wpf;component/Resources/Images/Menu/Close.png",
            };
        }

        public IEnumerable<LookupItem> GetAllMenuOptions()
        {
            return GetMenuOptions().Concat(GetSettingsOptions());
        }
    }
}
