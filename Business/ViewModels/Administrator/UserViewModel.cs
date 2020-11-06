using Business.Events.Administrator;
using Business.Interfaces.Administrator;
using Business.ViewModels.Main;
using Business.Wrappers;
using MahApps.Metro.Controls.Dialogs;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Business.ViewModels.Administrator
{
    public class UserViewModel : ViewModelBase, IUserViewModel
    {

        #region Properties

        public ObservableCollection<UserWrapper> Users { get; set; }

        #endregion

        #region Commands

        public ICommand RefreshCommand { get; set; }

        public ICommand NewUserCommand { get; set; }

        public ICommand UpdateUserCommand { get; set; }

        public ICommand RemoveUserCommand { get; set; }

        #endregion

        private ICollection<Role> _roles;
        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IUserRepository _userRepository;
        private readonly IUserDetailViewModel _userDetailViewModel;
        public UserViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IUserRepository userRepository,
            IEventAggregator eventAggregator,
            IUserDetailViewModel userDetailViewModel)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _userRepository = userRepository;
            _userDetailViewModel = userDetailViewModel;

            Users = new ObservableCollection<UserWrapper>();

            RefreshCommand = new DelegateCommand(Load, () => !IsBusy);
            NewUserCommand = new DelegateCommand(OnNewUserExecute, () => !IsBusy);
            UpdateUserCommand = new DelegateCommand<UserWrapper>(OnUpdateUserExecute);
            RemoveUserCommand = new DelegateCommand<int?>(OnRemoveUserExecute);

            eventAggregator.GetEvent<AfterUserSavedEvent>().Subscribe(OnAfterUserSaved);
        }

        public async void Load()
        {
            try
            {
                var restaurantId = _dataService.Restaurant.Id;
                var httpResponse = await ShowProgressAsync(async () => await _userRepository.GetSystemUsersAsync(restaurantId));
                if (httpResponse == null) return;

                if (httpResponse.IsSuccess)
                {
                    var users = httpResponse.Value.Users;
                    _roles = httpResponse.Value.Roles.ToList();

                    Users.Clear();
                    foreach (var user in users)
                    {
                        Users.Add(new UserWrapper(user));
                    }
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
            finally
            {
                ((DelegateCommand)RefreshCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)NewUserCommand).RaiseCanExecuteChanged();
            }
        }

        private void OnUpdateUserExecute(UserWrapper user)
        {
            if (user == null) return;
            _userDetailViewModel.Open(user.Model, _roles);
        }

        private async void OnRemoveUserExecute(int? id)
        {
            try
            {
                if (!id.HasValue) return;
                var result = await _dialogService.AskQuestionAsync("¿Estas seguro de querer eliminar este usuario?", "Elimiar Usuario");
                if (result == MessageDialogResult.Negative) return;

                var httpResponse = await ShowProgressAsync(async () => await _userRepository.DeleteUserAsync(id.Value));
                if (httpResponse.IsSuccess)
                {
                    var user = Users.Single(x => x.Id == httpResponse.Value);
                    Users.Remove(user);
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

        private void OnNewUserExecute() => _userDetailViewModel.Open(new User(), _roles);

        private void OnAfterUserSaved(User u)
        {
            var user = Users.SingleOrDefault(x => x.Id == u.Id);
            if (user == null) Users.Add(new UserWrapper(u));
            else user.Update(u);
        }
    }
}
