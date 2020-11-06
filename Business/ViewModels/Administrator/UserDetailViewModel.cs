using Business.Events.Administrator;
using Business.Interfaces.Administrator;
using Business.ViewModels.Main;
using Business.Wrappers;
using Microsoft.Win32;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Extensions;
using Service.Interfaces;
using Service.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Business.ViewModels.Administrator
{
    public class UserDetailViewModel : ModalBase, IUserDetailViewModel
    {
        #region Properties
        private UserWrapper _user;

        public UserWrapper User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ObservableCollection<Role> Roles { get; set; }

        #endregion

        #region Commands     

        public ICommand LoadImageCommand { get; set; }

        public ICommand DeleteImageCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IUserRepository _userRepository;
        private readonly IEventAggregator _eventAggregator;
        public UserDetailViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IUserRepository userRepository,
            IEventAggregator eventAggregator)
            : base(dialogService,eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _userRepository = userRepository;
            _eventAggregator = eventAggregator;

            Roles = new ObservableCollection<Role>();

            LoadImageCommand = new DelegateCommand(OnLoadImageExecute);
            DeleteImageCommand = new DelegateCommand(OnDeleteImageExecute);
        }

        public void Open(User user, ICollection<Role> roles)
        {
            InitializeUser(user);
            InitializeRoles(user.Roles, roles);
            IsOpen = true;
        }

        private void InitializeUser(User model)
        {

            User = new UserWrapper(model);

            User.PropertyChanged -= UserWrapperOnPropertyChanged;
            User.PropertyChanged += UserWrapperOnPropertyChanged;

            User.Name = model.Name;
            User.Email = model.Email;
            User.LastName = model.LastName;
            User.Password = model.Password;
            User.Image = model.Image;

            ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
        }

        private void InitializeRoles(ICollection<int> userRoles, ICollection<Role> roles)
        {
            Roles.Clear();
            foreach (var role in roles)
            {
                role.IsChecked = userRoles?.Any(id => id == role.Id) ?? false;
                Roles.Add(role);
            }
        }

        private void UserWrapperOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(User.HasErrors))
            {
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
            }
        }

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

        private void OnDeleteImageExecute()
        {
            User.Image = RestaurantImages.Profile.ImgUrlToByteArray();
        }

        protected override async void OnOkExecute()
        {
            try
            {
                if (Roles.Any(x => x.IsChecked))
                {
                    User.Model.RestaurantId = _dataService.Restaurant.Id;
                    User.Model.Roles = Roles.Where(x => x.IsChecked).Select(x => x.Id).ToArray();
                    var httpResponse = await ShowProgressAsync(async () => await _userRepository.SaveUserAsync(User.Model));
                    if (httpResponse.IsSuccess)
                    {
                        _eventAggregator.GetEvent<AfterUserSavedEvent>().Publish(httpResponse.Value);
                        OnCloseExecute();
                    }
                    else
                    {
                        await _dialogService.ShowMessageAsync(httpResponse.Message, httpResponse.Title);
                    }
                }
                else
                {
                    await _dialogService.ShowMessageAsync("Al menos un rol debe ser agregado");
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        protected override bool OnOkCanExecute() => User != null && !User.HasErrors;

        protected override void ClearValues() => User = null;

    }
}
