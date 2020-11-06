using Business.Events.Administrator;
using Business.Interfaces.Home;
using Business.ViewModels.Main;
using Business.Wrappers;
using Microsoft.Win32;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Extensions;
using Service.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Business.ViewModels.Home
{
    public class UserInformationViewModel : FlyoutBase, IUserInformationViewModel
    {

        #region Properties

        private UserWrapper _user;
        public UserWrapper User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        #endregion

        #region Commands

        public ICommand LoadImageCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IUserRepository _userRepository;
        private readonly IEventAggregator _eventAggregator;
        public UserInformationViewModel(
            IDialogService dialogService,
            IUserRepository userRepository,
            IDataService dataService,
            IEventAggregator eventAggregator,
            ILogService logService)
            : base(nameof(UserInformationViewModel), eventAggregator, dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _userRepository = userRepository;
            _eventAggregator = eventAggregator;

            LoadImageCommand = new DelegateCommand(OnLoadImageExecute);
        }

        public void Load()
        {
            var infoUser = new User(_dataService.User);
            User = new UserWrapper(infoUser);
            User.PropertyChanged -= UserOnPropertyChanged;
            User.PropertyChanged += UserOnPropertyChanged;

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void UserOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(User.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        protected override async void OnSaveExecute()
        {
            try
            {
                var httpResponse = await ActionAsync(async () => await _userRepository.UpdateUserInfoAsync(User.Model));
                if (httpResponse.IsSuccess)
                {
                    _dataService.SetUser(User.Model);
                    _eventAggregator.GetEvent<AfterUserInfoSavedEvent>().Publish();
                    OnCloseExecute();
                }
                else
                {
                    await _dialogService.ShowMessageAsync(httpResponse.Message, httpResponse.Title);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        protected override bool OnCanSaveExecute() => User != null && !_user.HasErrors;

        private void OnLoadImageExecute()
        {
            var openFile = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "All | *.jpg; *.jpeg; *.png"
            };

            if (openFile.ShowDialog() == true)
            {
                User.Image = openFile.FileName.ImgUrlToByteArray();
            }
        }
    }
}
