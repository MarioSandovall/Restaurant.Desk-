using Business.Interfaces.Administrator;
using Model.Models;
using Model.Utils;
using Service.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Business.ViewModels.Administrator
{
    public class AdminViewModel : ViewModelBase, IAdminViewModel
    {

        #region Properties

        private AdminMenu _selectedOption;

        public AdminMenu SelectedOption
        {
            get => _selectedOption;
            set
            {
                if (SelectedOption == value) return;
                SetProperty(ref _selectedOption, value);
                if (SelectedOption != null) OnSelectedOptionChanged();
            }
        }

        private IAdminPanel _currentViewModel;
        public IAdminPanel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ObservableCollection<AdminMenu> Menu { get; set; }

        public IUserViewModel UserViewModel { get; set; }

        public IUserDetailViewModel UserDetailViewModel { get; set; }

        public ICategoryViewModel CategoryViewModel { get; set; }

        public ICategoryDetailViewModel CategoryDetailViewModel { get; set; }

        public IProductViewModel ProductViewModel { get; set; }

        public IProductDetailViewModel ProductDetailViewModel { get; set; }

        public ITableViewModel TableViewModel { get; set; }

        public ITableDetailViewMoldel TableDetailViewMoldel { get; set; }

        public IRestaurantViewModel RestaurantViewModel { get; set; }

        public IOfficeDetailViewModel OfficeDetailViewModel { get; set; }

        public IOfficeViewModel OfficeViewModel { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        public AdminViewModel(
            ILogService logService,
            IUserViewModel userViewModel,
            IDialogService dialogService,
            ITableViewModel tableViewModel,
            IOfficeViewModel officeViewModel,
            IProductViewModel productViewModel,
            ICategoryViewModel categoryViewModel,
            IRestaurantViewModel restaurantViewModel,
            IUserDetailViewModel userDetailViewModel,
            ITableDetailViewMoldel tableDetailViewMoldel,
            IProductDetailViewModel productDetailViewModel,
            ICategoryDetailViewModel categoryDetailViewModel,
            IOfficeDetailViewModel officeDetailViewModel)
            : base(dialogService)
        {
            _logService = logService;
            _dialogService = dialogService;

            TableViewModel = tableViewModel;
            UserViewModel = userViewModel;
            ProductViewModel = productViewModel;
            OfficeViewModel = officeViewModel;
            RestaurantViewModel = restaurantViewModel;
            CategoryViewModel = categoryViewModel;
            UserDetailViewModel = userDetailViewModel;
            TableDetailViewMoldel = tableDetailViewMoldel;
            ProductDetailViewModel = productDetailViewModel;
            CategoryDetailViewModel = categoryDetailViewModel;
            OfficeDetailViewModel = officeDetailViewModel;
        }

        public void Load()
        {
            if (Menu == null) InitializateMenu();
            SelectedOption = Menu.First();
        }

        private void InitializateMenu()
        {
            Menu = new ObservableCollection<AdminMenu>()
            {
                new AdminMenu(){ Image="", Name = "Productos", Action =  AdminMenuAction.Products},
                new AdminMenu(){ Image="", Name = "Categorias",  Action =  AdminMenuAction.Categories},
                new AdminMenu(){ Image="", Name = "Mesas",  Action =  AdminMenuAction.Tables},
                new AdminMenu(){ Image="", Name = "Usuarios",  Action =  AdminMenuAction.Users},
                new AdminMenu(){ Image="", Name = "Sucursales ",  Action =  AdminMenuAction.Offices},
                new AdminMenu(){ Image="", Name = "Restaurante",  Action =  AdminMenuAction.Restaurant},
            };
        }

        private async void OnSelectedOptionChanged()
        {
            try
            {
                switch (SelectedOption.Action)
                {
                    case AdminMenuAction.Products:
                        CurrentViewModel = ProductViewModel;
                        ProductViewModel.Load();
                        break;
                    case AdminMenuAction.Categories:
                        CurrentViewModel = CategoryViewModel;
                        CategoryViewModel.Load();
                        break;
                    case AdminMenuAction.Users:
                        CurrentViewModel = UserViewModel;
                        UserViewModel.Load();
                        break;
                    case AdminMenuAction.Tables:
                        CurrentViewModel = TableViewModel;
                        TableViewModel.Load();
                        break;
                    case AdminMenuAction.Offices:
                        CurrentViewModel = OfficeViewModel;
                        OfficeViewModel.Load();
                        break;
                    case AdminMenuAction.Restaurant:
                        CurrentViewModel = RestaurantViewModel;
                        RestaurantViewModel.Load();
                        break;
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }
    }
}
