using Business.Events.Administrator;
using Business.Events.Home;
using Business.Interfaces.Home;
using MahApps.Metro;
using Prism.Events;
using Prism.Mvvm;
using Service.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

namespace Business.ViewModels.Home
{
    public class HomeViewModel : BindableBase, IHomeViewModel
    {

        #region Properties

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private string _officeName;
        public string OfficeName
        {
            get => _officeName;
            set => SetProperty(ref _officeName, value);
        }

        private string _restaurantName;
        public string RestaurantName
        {
            get => _restaurantName;
            set => SetProperty(ref _restaurantName, value);
        }

        private byte[] _image;
        public byte[] Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public ObservableCollection<NavigationItemViewModel> MenuItems { get; set; }

        #endregion

        private readonly IUserService _userService;
        private readonly IWindowService _windowService;
        private readonly ILookupServices _lookupServices;
        private readonly IEventAggregator _eventAggregator;

        public HomeViewModel(
            IUserService userService,
            IWindowService windowService,
            ILookupServices lookupServices,
            IEventAggregator eventAggregator)
        {
            _userService = userService;
            _windowService = windowService;
            _lookupServices = lookupServices;
            _eventAggregator = eventAggregator;

            MenuItems = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<UpdateAccentColorEvent>().Subscribe(OnUpdateAccentColor);
            _eventAggregator.GetEvent<AfterUserInfoSavedEvent>().Subscribe(InitializeUserInfo);
        }

        public void Load()
        {
            InitializeUserInfo();
            InitializeMenu();
        }

        private void InitializeUserInfo()
        {
            Image = _userService.Image;
            FullName = _userService.FullName;
            OfficeName = _userService.OfficeName;
            RestaurantName = _userService.RestaurantName;
        }
        private void OnUpdateAccentColor()
        {
            MenuItems.Clear();
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            if (MenuItems.Count == 0)
            {
                var selectedColor = string.Empty;
                var accentColor = ThemeManager.DetectAppStyle(_windowService.Window).Item2.Resources["AccentColor"].ToString();

                var menuOptions = _lookupServices.GetAllMenuOptions().ToList();
                foreach (var menuOption in menuOptions)
                {

                    SelectAccentColor(ref selectedColor, accentColor);

                    var NavigationItem = new NavigationItemViewModel(_eventAggregator, menuOption, selectedColor);

                    MenuItems.Add(NavigationItem);
                }
            }
        }

        private void SelectAccentColor(ref string currentColor, string secondaryColor)
        {
            const string PrimaryColor = "#FF444444";
            currentColor = currentColor == secondaryColor ? PrimaryColor : secondaryColor;
        }

    }
}
