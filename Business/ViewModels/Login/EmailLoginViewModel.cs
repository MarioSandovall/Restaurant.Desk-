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
                ((DelegateCommand)ValidateCommand).RaiseCanExecuteChanged();
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

        public ICommand ValidateCommand { get; set; }

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
            _dialogService = dialogService;
            _repository = repository;
            _eventAggregator = eventAggregator;

            ValidateCommand = new DelegateCommand(OnValidateExecute, OnValidateCanExecute);
        }

        public void Load()
        {
            Email = string.Empty;
            IsFocusable = true;
            OnValidateExecute();
        }

        private async void OnValidateExecute()
        {
            try
            {
                var userAccount = await _repository.GetUserAccountAsync(Email);

                _eventAggregator.GetEvent<EmailLoginValidEvent>().Publish(userAccount);
                _eventAggregator.GetEvent<BeforeNavigationEvent>().Publish(MenuAction.GoToLogin);

            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        private bool OnValidateCanExecute() => !string.IsNullOrEmpty(Email) && Email.IsEmailValid();

    }
}
