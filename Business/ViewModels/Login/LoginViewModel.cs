using Business.Events.Home;
using Business.Events.Login;
using Business.Interfaces.Login;
using Business.Wrappers;
using Model.Utils;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Business.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {

        #region Properties

        private UserWrapper _user;

        public UserWrapper User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public IOfficeChooserViewModel OfficeChooserViewModel { get; set; }
        #endregion

        #region Commands

        public ICommand LoginCommand { get; set; }
        public ICommand ReturnCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IUserRepository _userRepository;
        private readonly IEventAggregator _eventAggregator;

        public LoginViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IUserRepository userRepository,
            IEventAggregator eventAggregator,
            IOfficeChooserViewModel officeChooserViewModel)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _userRepository = userRepository;
            _eventAggregator = eventAggregator;

            OfficeChooserViewModel = officeChooserViewModel;

            LoginCommand = new DelegateCommand<object>(OnLoginExecute);
            ReturnCommand = new DelegateCommand(OnReturnExecute);

            _eventAggregator.GetEvent<EmailLoginValidEvent>().Subscribe((model) => User = new UserWrapper(model));

        }

        public void Load()
        {
            OnLoginExecute(new PasswordBox() { Password = "hola" });
        }

        private async void OnLoginExecute(object pass)
        {
            try
            {
                var password = pass as PasswordBox;
                if (string.IsNullOrEmpty(password?.Password))
                {
                    await _dialogService.ShowMessageAsync("Falta información", "La contraseña es requerida");
                    return;
                }

                var httpReponse = await ActionAsync(async () => await _userRepository.LoginAsync(User.Id, password?.Password));
                if (httpReponse.IsSuccess)
                {
                    _dataService.SetUser(httpReponse.Value.User);
                    _dataService.SetRestaurant(httpReponse.Value.Restaurant);
                    _dataService.SetBranchOffices(httpReponse.Value.BranchOffices);
                    _eventAggregator.GetEvent<BeforeNavigationEvent>().Publish(MenuAction.GoToHome);
                }
                else
                {
                    await _dialogService.ShowMessageAsync(httpReponse.Message, httpReponse.Title);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        private void OnReturnExecute()
        {
            _eventAggregator.GetEvent<BeforeNavigationEvent>().Publish(MenuAction.GoToEmailLogin);
        }


    }
}
