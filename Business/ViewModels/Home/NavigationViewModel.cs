using Business.Events.Home;
using Business.Interfaces.Home;
using Business.Interfaces.Main;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Service.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Business.ViewModels.Home
{
    public class NavigationViewModel : BindableBase, INavigationViewModel
    {
        #region Properties

        private IViewModelBase _currentViewModel;
        public IViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private Visibility _menuVisibility;
        public Visibility MenuVisibility
        {
            get { return _menuVisibility; }
            set { SetProperty(ref _menuVisibility, value); }
        }

        private bool _isHambugerMenuOpen;
        public bool IsHambugerMenuOpen
        {
            get { return _isHambugerMenuOpen; }
            set { SetProperty(ref _isHambugerMenuOpen, value); }
        }

        private LookupItem _selectedLocation;
        public LookupItem SelectedLocation
        {
            get { return _selectedLocation; }
            set { SetProperty(ref _selectedLocation, value); }
        }

        public ObservableCollection<LookupItem> MenuItems { get; set; }
        public ObservableCollection<LookupItem> Options { get; set; }

        #endregion

        #region Commands

        public ICommand NavCommand { get; set; }

        #endregion

        private readonly IEventAggregator _eventAggregator;
        private readonly ILookupServices _lookupService;
        private readonly IDataService _dataService;

        public NavigationViewModel(
            IEventAggregator eventAggregator,
            ILookupServices lookupService,
            IDataService dataService)

        {
            _lookupService = lookupService;
            _eventAggregator = eventAggregator;
            _dataService = dataService;

            MenuItems = new ObservableCollection<LookupItem>();
            Options = new ObservableCollection<LookupItem>();

            NavCommand = new DelegateCommand(OnSelectLocation);

            _eventAggregator.GetEvent<VisibleMenuEvent>().Subscribe(OnVisibleMenu);
            _eventAggregator.GetEvent<AfterNavigationEvent>().Subscribe(OnAfterNavigationEvent);
        }



        public void Load()
        {
            if (MenuItems.Count > 0) return;

            var lookup = _lookupService.GetMenuItems().ToList();
            var isAdmin = _dataService.IsAdmin;

            foreach (var menu in lookup)
            {
                if (menu.IsForAdmin && !isAdmin) continue;
                MenuItems.Add(menu);
            }

            lookup = _lookupService.GetOptions().ToList();
            foreach (var option in lookup)
            {
                if (option.IsForAdmin && !isAdmin) continue;
                Options.Add(option);
            }
        }

        private void OnSelectLocation()
        {
            if (SelectedLocation == null) return;
            _eventAggregator.GetEvent<BeforeNavigationEvent>().Publish(SelectedLocation.Action);
        }

        private void OnAfterNavigationEvent(AfterNavigationEventArgs args)
        {
            CurrentViewModel = args.ViewModel;
            IsHambugerMenuOpen = args.IsHamburgerMenuOpen;
            OnVisibleMenu(args.IsMenuVisible);
        }


        private void OnVisibleMenu(bool isMenuVisible)
        {
            MenuVisibility = isMenuVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
