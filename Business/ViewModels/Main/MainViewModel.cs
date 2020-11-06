using Business.Events.Home;
using Business.Interfaces.Administrator;
using Business.Interfaces.Home;
using Business.Interfaces.Login;
using Business.Interfaces.Main;
using Business.Interfaces.Register;
using MahApps.Metro.Controls.Dialogs;
using Model.Utils;
using Prism.Events;
using Prism.Mvvm;
using Service.Interfaces;
using System;
using System.Net.NetworkInformation;
using System.Windows;
using Business.Utils;

namespace Business.ViewModels.Main
{
    public class MainViewModel : BindableBase
    {
        #region Properties

        private bool _isSettingsFlyoutOpen;
        public bool IsSettingsFlyoutOpen
        {
            get => _isSettingsFlyoutOpen;
            set => SetProperty(ref _isSettingsFlyoutOpen, value);
        }

        private bool _isUserInfoFlyoutOpen;
        public bool IsUserInfoFlyoutOpen
        {
            get => _isUserInfoFlyoutOpen;
            set => SetProperty(ref _isUserInfoFlyoutOpen, value);
        }

        private bool _isNotNetWorkAvailable;
        public bool IsNotNetWorkAvailable
        {
            get => _isNotNetWorkAvailable;
            set => SetProperty(ref _isNotNetWorkAvailable, value);
        }

        public IHomeViewModel HomeViewModel { get; set; }

        public IAdminViewModel AdminViewModel { get; set; }

        public ILoginViewModel LoginViewModel { get; set; }

        public IStartUpViewModel StartUpViewModel { get; set; }

        public ISettingsViewModel SettingsViewModel { get; set; }

        public INavigationViewModel NavigationViewModel { get; set; }

        public ICashRegisterViewModel CashRegisterViewModel { get; set; }

        public ISoftwareUpdateViewModel SoftwareUpdateViewModel { get; set; }

        public IUserInformationViewModel UserInformationViewModel { get; set; }

        public IEmailLoginViewModel EmailLoginViewModel { get; set; }

        #endregion

        private IViewModelBase _currentViewModel;
        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        private readonly IConfigService _configService;
        private readonly IPrintingService _printingService;
        private readonly IEventAggregator _eventAggregator;

        public MainViewModel(
           ILogService logService,
            IDialogService dialogService,
            IHomeViewModel homeViewModel,
            IConfigService configService,
            ILoginViewModel loginViewModel,
            IAdminViewModel adminViewModel,
            IEventAggregator eventAggregator,
            IPrintingService printingService,
            IStartUpViewModel startUpViewModel,
            ISettingsViewModel settingsViewModel,
            IEmailLoginViewModel emailLoginViewModel,
            INavigationViewModel navigationViewModel,
            ICashRegisterViewModel cashRegisterViewModel,
            ISoftwareUpdateViewModel softwareUpdateViewModel,
            IUserInformationViewModel userInformationViewModel)
        {
            _logService = logService;
            _dialogService = dialogService;
            _configService = configService;
            _eventAggregator = eventAggregator;
            _printingService = printingService;

            HomeViewModel = homeViewModel;
            AdminViewModel = adminViewModel;
            LoginViewModel = loginViewModel;
            StartUpViewModel = startUpViewModel;
            SettingsViewModel = settingsViewModel;
            NavigationViewModel = navigationViewModel;
            EmailLoginViewModel = emailLoginViewModel;
            CashRegisterViewModel = cashRegisterViewModel;
            UserInformationViewModel = userInformationViewModel;
            SoftwareUpdateViewModel = softwareUpdateViewModel;

            _eventAggregator.GetEvent<CloseFlyoutEvent>().Subscribe(OnCloseFlyout);
            _eventAggregator.GetEvent<CloseApplicationEvent>().Subscribe(CloseApplication);
            _eventAggregator.GetEvent<BeforeNavigationEvent>().Subscribe(OnBeforeNavigation);

            IsNotNetWorkAvailable = false;
            NetworkChange.NetworkAddressChanged += (s, a) => Application.Current.Dispatcher.Invoke(() => IsNotNetWorkAvailable = !NetworkHelper.IsAvailable);
        }

        public void Load()
        {
            try
            {
                _printingService.Start();
                OnBeforeNavigation(MenuAction.GoToStartUp);
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        public void Theme() => _configService.LoadTheme();

        private void OnBeforeNavigation(MenuAction location)
        {
            var isMenuVisible = true;
            switch (location)
            {
                case MenuAction.GoToStartUp:
                    isMenuVisible = false;
                    _currentViewModel = StartUpViewModel;
                    StartUpViewModel.Load();
                    break;
                case MenuAction.GoToEmailLogin:
                    isMenuVisible = false;
                    _currentViewModel = EmailLoginViewModel;
                    break;
                case MenuAction.GoToLogin:
                    isMenuVisible = false;
                    _currentViewModel = LoginViewModel;
                    break;
                case MenuAction.GoToHome:
                    _currentViewModel = HomeViewModel;
                    NavigationViewModel.Load();
                    HomeViewModel.Load();
                    break;
                case MenuAction.GoToCashRegister:
                    _currentViewModel = CashRegisterViewModel;
                    CashRegisterViewModel.Load();
                    break;
                case MenuAction.GoToAdmin:
                    _currentViewModel = AdminViewModel;
                    AdminViewModel.Load();
                    break;
                case MenuAction.GoToSettings:
                    SettingsViewModel.Load();
                    IsSettingsFlyoutOpen = true;
                    break;
                case MenuAction.UpdateApplication:
                    SoftwareUpdateViewModel.CheckForUpdates();
                    break;
                case MenuAction.GoToUserInformation:
                    UserInformationViewModel.Load();
                    IsUserInfoFlyoutOpen = true;
                    break;
                case MenuAction.Exit:
                    CloseApplication();
                    break;
            }

            _eventAggregator.GetEvent<AfterNavigationEvent>()
                .Publish(new AfterNavigationEventArgs()
                {
                    IsMenuVisible = isMenuVisible,
                    IsHamburgerMenuOpen = false,
                    ViewModel = _currentViewModel
                });
        }

        private void OnCloseFlyout(string viewModel)
        {
            switch (viewModel)
            {
                case nameof(SettingsViewModel):
                    IsSettingsFlyoutOpen = false;
                    break;
                case nameof(UserInformationViewModel):
                    IsUserInfoFlyoutOpen = false;
                    break;
            }
        }

        public async void CloseApplication()
        {
            var result = await _dialogService.AskQuestionAsync("Are you sure you want to close the application?");
            if (result == MessageDialogResult.Negative) return;
            Application.Current?.Shutdown();
        }

    }
}
