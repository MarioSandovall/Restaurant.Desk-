using Business.Events.Home;
using Business.Events.Login;
using Business.Extensions;
using Business.Interfaces.Administrator;
using Business.ViewModels.Main;
using Model.Utils;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Windows.Input;

namespace Business.ViewModels.Login
{
    public class EmailLoginViewModel : ViewModelBase, IEmailLoginViewModel
    {

        #region Properties

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                ((DelegateCommand)GetUserAccountCommand).RaiseCanExecuteChanged();
            }
        }



        private bool _isFocusable;
        public bool IsFocusable
        {
            get => _isFocusable;
            set => SetProperty(ref _isFocusable, value);
        }

        #endregion

        #region Commands

        public ICommand GetUserAccountCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        private readonly IAccountRepository _repository;
        private readonly IEventAggregator _eventAggregator;

        public EmailLoginViewModel(
            ILogService logService,
            IDialogService dialogService,
            IAccountRepository repository,
            IEventAggregator eventAggregator)
            : base(dialogService)
        {
            _logService = logService;
            _repository = repository;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            GetUserAccountCommand = new DelegateCommand(OnGetUserAccountExecute, OnGetUserAccountCanExecute);
        }

        public void Load()
        {
            Email = string.Empty;
            IsFocusable = true;
            OnGetUserAccountExecute();
        }

        private async void OnGetUserAccountExecute()
        {
            try
            {
                if (await ShowProgressAsync(async () => await _repository.ExistsAsync(Email)))
                {
                    var userAccount = await ShowProgressAsync(async () => await _repository.GetUserAccountAsync(Email));

                    _eventAggregator.GetEvent<EmailLoginValidEvent>().Publish(userAccount);
                    _eventAggregator.GetEvent<BeforeNavigationEvent>().Publish(MenuAction.GoToLogin);
                }
                else
                {
                    await _dialogService.ShowMessageAsync("Email not found");
                }

            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                while (ex.InnerException != null) ex = ex.InnerException;
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        private bool OnGetUserAccountCanExecute() => !string.IsNullOrEmpty(Email) && Email.IsEmailValid();

    }
}
