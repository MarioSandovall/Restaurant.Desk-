using Model.Models;
using Model.Utils;
using Service.Interfaces;
using System.Collections.Generic;

namespace Service.Services
{
    public class LookupService : ILookupServices
    {
        private readonly IDataService _dataService;
        public LookupService(
            IDataService dataService)
        {
            _dataService = dataService;
        }

        public IEnumerable<LookupItem> GetMenuItems()
        {
            yield return new LookupItem()
            {
                Name = "Inicio",
                IsForAdmin = false,
                Action = MenuAction.GoToHome,
                Image = "/Restaurant.DeskApp;component/Resources/Images/Menu/Home.png",
            };

            if (_dataService.IsCasher)
            {
                yield return new LookupItem()
                {
                    Name = "Caja",
                    IsForAdmin = false,
                    Action = MenuAction.GoToCashRegister,
                    Image = "/Restaurant.DeskApp;component/Resources/Images/Menu/CountingMachine.png",
                };
            }

            if (_dataService.IsAdmin)
            {
                yield return new LookupItem()
                {
                    Name = "Administrador",
                    IsForAdmin = true,
                    Action = MenuAction.GoToAdmin,
                    Image = "/Restaurant.DeskApp;component/Resources/Images/Menu/Admin.png",
                };
            }

            yield return new LookupItem()
            {
                Name = "Usuario",
                IsForAdmin = false,
                Action = MenuAction.GoToUserInformation,
                Image = "/Restaurant.DeskApp;component/Resources/Images/Menu/UserProfile.png",
            };
        }

        public IEnumerable<LookupItem> GetOptions()
        {
            yield return new LookupItem()
            {
                Name = "Actualizar",
                IsForAdmin = false,
                Action = MenuAction.UpdateApplication,
                Image = "/Restaurant.DeskApp;component/Resources/Images/Menu/Update.png",
            };

            yield return new LookupItem()
            {
                Name = "Configuraciones",
                IsForAdmin = false,
                Action = MenuAction.GoToSettings,
                Image = "/Restaurant.DeskApp;component/Resources/Images/Menu/Settings.png",
            };

            yield return new LookupItem()
            {
                Name = "Salir",
                IsForAdmin = false,
                Action = MenuAction.Exit,
                Image = "/Restaurant.DeskApp;component/Resources/Images/Menu/Close.png",
            };
        }
    }
}
