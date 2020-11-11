using Business.Interfaces.Home;
using Business.ViewModels.Main;
using Business.Wrappers.Home;
using Microsoft.Win32;
using Model.Models.Home;
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

        private ProfileUserWrapper _profileUser;
        public ProfileUserWrapper ProfileUser
        {
            get => _profileUser;
            set => SetProperty(ref _profileUser, value);
        }

        #endregion

        #region Commands

        public ICommand LoadImageCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;
        private readonly IUserRepository _userRepository;
        private readonly IEventAggregator _eventAggregator;
        public UserInformationViewModel(
            IUserService userService,
            IDialogService dialogService,
            IUserRepository userRepository,
            IEventAggregator eventAggregator,
            ILogService logService)
            : base(nameof(UserInformationViewModel), eventAggregator, dialogService)
        {
            _logService = logService;
            _userService = userService;
            _dialogService = dialogService;
            _userRepository = userRepository;
            _eventAggregator = eventAggregator;

            LoadImageCommand = new DelegateCommand(OnLoadImageExecute);
        }

        public void Load()
        {
            var userProfile = new ProfileUser
            {
                Id = _userService.Id,
                Name = _userService.Name,
                Image = _userService.Image,
                LastName = _userService.LastName
            };

            ProfileUser = new ProfileUserWrapper(userProfile);

            ProfileUser.PropertyChanged -= UserOnPropertyChanged;
            ProfileUser.PropertyChanged += UserOnPropertyChanged;

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void UserOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProfileUser.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        protected override async void OnSaveExecute()
        {
            try
            {
                //var httpResponse = await ShowProgressAsync(async () => await _userRepository.UpdateUserInfoAsync(ProfileUser.Model));
                //if (httpResponse.IsSuccess)
                //{
                //    //_dataService.SetUser(ProfileUser.Model);
                //    _eventAggregator.GetEvent<AfterUserInfoSavedEvent>().Publish();
                //    OnCloseExecute();
                //}
                //else
                //{
                //    await _dialogService.ShowMessageAsync(httpResponse.Message, httpResponse.Title);
                //}
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        protected override bool OnCanSaveExecute() => ProfileUser != null && !_profileUser.HasErrors;

        private void OnLoadImageExecute()
        {
            var openFile = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "All | *.jpg; *.jpeg; *.png"
            };

            if (openFile.ShowDialog() == true)
            {
                ProfileUser.Image = openFile.FileName.ImgUrlToByteArray();
            }
        }
    }
}
