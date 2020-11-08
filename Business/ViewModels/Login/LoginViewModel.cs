using Business.Events.Home;
using Business.Events.Login;
using Business.Interfaces.Login;
using Business.ViewModels.Main;
using Business.Wrappers;
using Model.Models.Login;
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

        private UserAccountWrapper _userAccount;
        public UserAccountWrapper UserAccount
        {
            get => _userAccount;
            set => SetProperty(ref _userAccount, value);
        }

        public IOfficeChooserViewModel OfficeChooserViewModel { get; set; }

        #endregion

        #region Commands

        public ICommand LoginCommand { get; set; }

        public ICommand ReturnCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly ILoginRepository _repository;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;

        public LoginViewModel(
            ILogService logService,
            IUserService userService,
            ILoginRepository repository,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IOfficeChooserViewModel officeChooserViewModel)
            : base(dialogService)
        {
            _logService = logService;
            _repository = repository;
            _userService = userService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            OfficeChooserViewModel = officeChooserViewModel;

            LoginCommand = new DelegateCommand<object>(OnLoginExecute);

            ReturnCommand = new DelegateCommand(OnReturnExecute);

            _eventAggregator.GetEvent<UserAccountEvent>().Subscribe((userAccount) =>
            {
                UserAccount = new UserAccountWrapper(userAccount);
            });

        }


        private async void OnLoginExecute(object pass)
        {
            try
            {
                var passwordInput = pass as PasswordBox;
                var password = passwordInput?.Password;

                if (string.IsNullOrEmpty(password))
                {
                    await _dialogService.ShowMessageAsync("Password is required", "Missing Information");
                    return;
                }

                await ShowProgressAsync(async () =>
                {

                    var AuthenticateModel = new AuthenticateModel
                    {
                        Password = password,
                        Email = UserAccount.Email
                    };

                    if (await _repository.ExistsAsync(AuthenticateModel))
                    {
                        var loggedUser = await _repository.GetUserAsync(AuthenticateModel);

                        _userService.SetUser(loggedUser);

                        _eventAggregator.GetEvent<BeforeNavigationEvent>().Publish(MenuAction.GoToHome);

                    }
                    else
                    {
                        await _dialogService.ShowMessageAsync("The password is not correct");
                    }

                });
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                while (ex.InnerException != null) ex = ex.InnerException;
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        private void OnReturnExecute()
        {
            _eventAggregator.GetEvent<BeforeNavigationEvent>().Publish(MenuAction.GoToEmailLogin);
        }


    }
}
