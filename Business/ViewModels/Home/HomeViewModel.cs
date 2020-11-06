using Business.Events;
using Business.Interfaces.Home;
using MahApps.Metro;
using Model.Models;
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

        private string _currentColor;
        private readonly IDataService _dataService;
        private readonly ILookupServices _lookupServices;
        private readonly IWindowService _windowService;
        private readonly IEventAggregator _eventAggregator;

        public HomeViewModel(
            ILookupServices lookupServices,
            IEventAggregator eventAggregator,
            IWindowService windowService,
            IDataService dataService)
        {
            _lookupServices = lookupServices;
            _eventAggregator = eventAggregator;
            _windowService = windowService;
            _dataService = dataService;

            _currentColor = string.Empty;
            MenuItems = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<UpdateAccentColorEvent>().Subscribe(InitializeMenu);
            _eventAggregator.GetEvent<AfterUserInfoSavedEvent>().Subscribe(InitializeUser);
            _eventAggregator.GetEvent<AfterRestaurantSavedEvent>().Subscribe(InitializeRestaurant);
        }

        public void Load()
        {
            InitializeUser();
            InitializeRestaurant();
            if (MenuItems.Count == 0) InitializeMenu();
        }

        private void InitializeUser()
        {
            Image = _dataService.User.Image;
            FullName = _dataService.User.Name;
        }

        private void InitializeRestaurant()
        {
            OfficeName = _dataService.CurrentOffice.Name;
            RestaurantName = _dataService.Restaurant.Name;
        }

        private void InitializeMenu()
        {
            MenuItems.Clear();
            var lookup = _lookupServices.GetMenuItems().ToList();
            var isAdmin = _dataService.IsAdmin;

            foreach (var item in lookup)
            {
                if (item.IsForAdmin && !isAdmin) continue;
                AddItemToMenu(item);
            }

            lookup = _lookupServices.GetOptions().ToList();
            foreach (var options in lookup)
            {
                if (options.IsForAdmin && !isAdmin) continue;
                AddItemToMenu(options);
            }
        }

        private void AddItemToMenu(LookupItem item)
        {
            _currentColor = _currentColor == SecondaryColor ? PrimaryColor : SecondaryColor;
            MenuItems.Add(new NavigationItemViewModel(_eventAggregator, item, _currentColor));
        }

        private const string PrimaryColor = "#FF444444";
        private string SecondaryColor => ThemeManager.DetectAppStyle(_windowService.Window).Item2.Resources["AccentColor"].ToString();

    }
}
